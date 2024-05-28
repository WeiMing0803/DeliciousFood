using AI.DeliciousFood.Core.Model;
using AI.DeliciousFood.Core.Server;
using AI.DeliciousFood.Web.Client.Helper;
using Aop.Api;
using Aop.Api.Domain;
using Aop.Api.Request;
using Aop.Api.Response;
using Aop.Api.Util;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Security.Claims;

namespace AI.DeliciousFood.Web.Client.Controllers
{
    public class AlipayController(IAlipayRepository alipayRepository,
        GlobalConfig globalConfig,
        WebSocketManagerHelper webSocketManager,
        AlipayConfigHelper alipayConfigHelper
        ) : CommonControllerBase
    {

        [HttpGet]
        public IActionResult GetMenberPrice()
        {
            var result = alipayRepository.GetMenberPriceAsync();
            return Ok(new
            {
                success = true,
                data = result
            });
        }

        /// <summary>
        /// 用于创建预支付二维码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        
        public async Task<IActionResult> AlipayTradePrecreate(Guid memberPriceGuid)
        {
            MemberPrice currentMemberPrice = await alipayRepository.GetMenberPriceByIdAsync(memberPriceGuid);

            AlipayConfig alipayConfig = new AlipayConfig
            {
                ServerUrl = alipayConfigHelper.ServerUrl,
                AppId = alipayConfigHelper.AppId,
                PrivateKey = alipayConfigHelper.PrivateKey,
                Format = alipayConfigHelper.Format,
                AlipayPublicKey = alipayConfigHelper.AlipayPublicKey,
                Charset = alipayConfigHelper.Charset,
                SignType = alipayConfigHelper.SignType
            };

            IAopClient alipayClient = new DefaultAopClient(alipayConfig);

            AlipayTradePrecreateRequest request = new AlipayTradePrecreateRequest();
            AlipayTradePrecreateModel model = new AlipayTradePrecreateModel();
            model.OutTradeNo = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            model.TotalAmount = currentMemberPrice.Price.ToString();
            model.Subject = currentMemberPrice.MemberName;
            model.PassbackParams = UserInfo.UserId.ToString();

            // 设置异步通知的URL
            request.SetNotifyUrl($"{alipayConfigHelper.SetNotifyUrl}alipay/notify");
            request.SetBizModel(model);

            AlipayTradePrecreateResponse response = alipayClient.Execute(request);
            if (!response.IsError)
            {
                return Ok(new
                {
                    success = true,
                    data = response.QrCode,
                    message = "调用成功"
                });
            }
            else
            {
                Log.Error("调用失败，错误信息：" + response.Body);
                return Ok(new
                {
                    success = false,
                    data = "",
                    message = "二维码加载失败"
                });
            }
        }


        /// <summary>
        /// 支付成功回调方法
        /// </summary>
        /// <returns></returns>
        [HttpPost("alipay/notify")]
        public async Task<IActionResult> AlipayNotify()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (var key in Request.Form.Keys)
            {
                dict[key] = Request.Form[key];
            }

            bool isVerified = AlipaySignature.RSACheckV1(dict, alipayConfigHelper.AlipayPublicKey, alipayConfigHelper.Charset, alipayConfigHelper.SignType, false);
            if (!isVerified)
            {
                Log.Error("alipay/notify: Invalid Signature");
                return BadRequest("Invalid Signature"); // 签名验证失败
            }

            string tradeStatus = Request.Form["trade_status"];
            if (tradeStatus == "TRADE_SUCCESS" || tradeStatus == "TRADE_FINISHED")
            {
                // 接收并处理支付宝通知
                // 如果支付成功:
                string userId = dict["passback_params"];
                await webSocketManager.SendPrivateMessage("支付成功", userId);

                // 处理交易完成后的业务逻辑
                return Ok("success"); // 对支付宝返回结果，以防重复发送通知
            }
            return Ok("failure");
        }

    }
}
