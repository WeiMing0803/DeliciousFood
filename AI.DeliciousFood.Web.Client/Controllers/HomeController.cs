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
    public class HomeController(IAlipayRepository accountRepository,
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