using AI.DeliciousFood.Core.Common.Model;
using AI.DeliciousFood.Core.Server;
using AI.DeliciousFood.Web.Client.Helper;
using AI.DeliciousFood.Web.Client.Models;
using Microsoft.AspNetCore.Mvc;
using AI.DeliciousFood.Core.Common.Model.Home;

namespace AI.DeliciousFood.Web.Client.Controllers;

public class HomeController(GlobalConfig globalConfig, WebSocketManagerHelper webSocketManager, IHomeRepository homeRepository) : CommonControllerBase
{

    public async Task<IActionResult> Index()
    {
        List<RecommendViewModel> monthly = await homeRepository.GetRecommend(RecommendType.Monthly);
        List<RecommendViewModel> hotList = await homeRepository.GetRecommend(RecommendType.HotList);
        RecommendCollectionViewModel viewModel = new()
        {
            Monthly = monthly,
            HotList = hotList
        };
        return View(viewModel);
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

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult NotFoundHtml()
    {
        return View();
    }
}