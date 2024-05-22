using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;

namespace AI.DeliciousFood.Web.Client.Helper
{
    public class WebSocketManagerHelper
    {
        private readonly ConcurrentDictionary<string, WebSocket> _sockets = new ConcurrentDictionary<string, WebSocket>();

        public async Task HandleConnectionAsync(HttpContext context, string userId)
        {
            if (!context.WebSockets.IsWebSocketRequest)
            {
                context.Response.StatusCode = 400;
                return;
            }

            WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
            _sockets.TryAdd(userId, webSocket); // 使用用户ID作为键值添加WebSocket到字典中

            await ReceiveMessage(webSocket, userId, async (result, buffer, userId) =>
            {
                if (result.MessageType == WebSocketMessageType.Text && result.EndOfMessage)
                {
                    await SendPrivateMessage(Encoding.UTF8.GetString(buffer), userId);
                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    await CloseSocket(userId); // 使用用户ID关闭对应的WebSocket连接
                }
            });
        }

        private async Task ReceiveMessage(WebSocket socket, string userId, Action<WebSocketReceiveResult, byte[], string> handleMessage)
        {
            byte[] buffer = new byte[1024 * 4];

            // 监听特定用户的WebSocket消息
            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                handleMessage(result, buffer, userId);  // 将 userId 传递给处理函数，以便知道是哪个用户的消息
            }
        }

        public async Task SendPrivateMessage(string message, string userId)
        {
            if (_sockets.TryGetValue(userId, out WebSocket socket) && socket.State == WebSocketState.Open)
            {
                await socket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(message)),
                    WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        private async Task CloseSocket(string id)
        {
            if (_sockets.TryRemove(id, out WebSocket socket))
            {
                await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by manager", CancellationToken.None);
                socket.Dispose();
            }
        }
    }
}
