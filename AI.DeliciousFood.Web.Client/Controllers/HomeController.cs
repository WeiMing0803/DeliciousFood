using AI.DeliciousFood.Core.Server;
using AI.DeliciousFood.Web.Client.Helper;
using AI.DeliciousFood.Web.Client.Models;
using Microsoft.AspNetCore.Mvc;

namespace AI.DeliciousFood.Web.Client.Controllers;

public class HomeController(IAlipayRepository accountRepository,
    GlobalConfig globalConfig, 
    WebSocketManagerHelper webSocketManager,
    AlipayConfigHelper alipayConfigHelper
    ) : CommonControllerBase
{

    public IActionResult Index()
    {
        //Log.Error("Hello World");
        //await webSocketManager.BroadcastMessage("支付成功");

        return View();
    }
    
    public IActionResult AboutUs()
    {
        return View();
    }


    [HttpGet("/ws")]
    public async Task Get()
    {
        await webSocketManager.HandleConnectionAsync(HttpContext, UserInfo.UserId.ToString());
    }


    public async Task<IActionResult> Privacy()
    {

        await webSocketManager.SendPrivateMessage("支付成功", UserInfo.UserId.ToString());
        return View();
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}