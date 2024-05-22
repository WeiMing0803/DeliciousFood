using AI.DeliciousFood.Core.Model;
using AI.DeliciousFood.Core.Server;
using AI.DeliciousFood.Web.Client.Helper;
using AI.DeliciousFood.Web.Client.Models;
using Aop.Api.Domain;
using Aop.Api.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace AI.DeliciousFood.Web.Client.Controllers
{
    public class HomeController(IAccountRepository accountRepository,
        GlobalConfig globalConfig, 
        WebSocketManagerHelper webSocketManager,
        AlipayConfigHelper alipayConfigHelper
        ) : CommonControllerBase
    {

        public async Task<IActionResult> Index()
        {
            var a = UserInfo;
            var b = alipayConfigHelper.AlipayPublicKey;
            //Log.Error("Hello World");
            //await webSocketManager.BroadcastMessage("支付成功");

            return View();
        }

        public IActionResult GetMenberPrice()
        {
            var result = accountRepository.GetMenberPriceAsync();
            return Ok(new
            {
                success = true,
                data = result
            });
        }

        public async Task<IActionResult> Privacy()
        {
            
            await webSocketManager.SendPrivateMessage("支付成功", UserInfo.UserId.ToString());
            return View();
        }
                
        [HttpPost("alipay/notify")]
        public async Task<IActionResult> AlipayNotify()
        {
            string charset = "UTF-8";

            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (var key in Request.Form.Keys)
            {
                dict[key] = Request.Form[key];
            }

            bool isVerified = AlipaySignature.RSACheckV1(dict, globalConfig.AlipayPublicKey, charset, globalConfig.SignType, false);
            if (!isVerified)
            {
                return BadRequest("Invalid Signature"); // 签名验证失败
            }

            string tradeStatus = Request.Form["trade_status"];
            if (tradeStatus == "TRADE_SUCCESS" || tradeStatus == "TRADE_FINISHED")
            {
                // 接收并处理支付宝通知
                // 如果支付成功:
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                await webSocketManager.SendPrivateMessage("支付成功", userId);


                // 处理交易完成后的业务逻辑
                return Ok("success"); // 对支付宝返回结果，以防重复发送通知
            }
            return Ok("failure");
        }

        [HttpGet("/ws")]
        public async Task Get()
        {
            await webSocketManager.HandleConnectionAsync(HttpContext, UserInfo.UserId.ToString());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}