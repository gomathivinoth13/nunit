////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	Controllers\PushController.cs
//
// summary:	Implements the push controller class
////////////////////////////////////////////////////////////////////////////////////////////////////

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SEG.ApiService.Models;
using SEG.ApiService.Models.Attributes;
using SEG.AzureLoyaltyDatabase;
using SEG.OffersLibrary.Controller;
using SEG.PushNotifications.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SEG.LoyaltyServiceWeb.Controllers
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   A controller for handling pushes. </summary>
    ///
    /// <remarks>   Mcdand, 2/20/2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public class PushController : Controller
    {
    //    SEG.PushNotifications.PushNotificationEngine engine;    ///< The engine
    //    IConfiguration Configuration;   ///< The configuration

    //    ////////////////////////////////////////////////////////////////////////////////////////////////////
    //    /// <summary>   Constructor. </summary>
    //    ///
    //    /// <remarks>   Mcdand, 2/20/2018. </remarks>
    //    ///
    //    /// <param name="configuration">    The configuration. </param>
    //    ////////////////////////////////////////////////////////////////////////////////////////////////////

    //    public PushController(IConfiguration configuration)
    //    {
    //        Configuration = configuration;
    //        engine = new PushNotifications.PushNotificationEngine(Configuration["Settings:Mobile:PushPayload:EndPoint"], Configuration["Settings:AzureLoyaltyApiEndpoint"], Configuration["Settings:Mobile:PushPayload:ApiKeys:Bilo"], Configuration["Settings:Mobile:PushPayload:ApiKeys:Harveys"], Configuration["Settings:Mobile:PushPayload:ApiKeys:WinnDixie"], Configuration["Settings:LoyaltyConnection"]);
    //    }

    //    ////////////////////////////////////////////////////////////////////////////////////////////////////
    //    /// <summary>
    //    /// (An Action that handles HTTP POST requests) subscribes the given request.
    //    /// </summary>
    //    ///
    //    /// <remarks>   Mcdand, 2/20/2018. </remarks>
    //    ///
    //    /// <param name="request">  The request. </param>
    //    ///
    //    /// <returns>   An asynchronous result that yields a SubscriptionResponse. </returns>
    //    ////////////////////////////////////////////////////////////////////////////////////////////////////

    //    [HttpPost]
    //    [Route("api/mobile/push/subscribe")]
    //    public async Task<SubscriptionResponse> Subscribe([FromBody]PushNotifications.Models.PushSubscribeRequest request)
    //    {
    //        return await engine.SubscribeAsync(request);
    //    }

    //    ////////////////////////////////////////////////////////////////////////////////////////////////////
    //    /// <summary>   (An Action that handles HTTP POST requests) un subscribe. </summary>
    //    ///
    //    /// <remarks>   Mcdand, 2/20/2018. </remarks>
    //    ///
    //    /// <param name="request">  The request. </param>
    //    ///
    //    /// <returns>   An asynchronous result that yields a SubscriptionResponse. </returns>
    //    ////////////////////////////////////////////////////////////////////////////////////////////////////

    //    [HttpPost]
    //    [Route("api/mobile/push/unsubscribe")]
    //    public async Task<SubscriptionResponse> UnSubscribe([FromBody]PushUnSubscribeRequest request)
    //    {
    //        return await engine.UnsubscribeAsync(request);
    //    }

    //    ////////////////////////////////////////////////////////////////////////////////////////////////////
    //    /// <summary>   (An Action that handles HTTP POST requests) at stores. </summary>
    //    ///
    //    /// <remarks>   Mcdand, 2/20/2018. </remarks>
    //    ///
    //    /// <param name="device_tokens">    The device tokens. </param>
    //    /// <param name="banner">           The banner. </param>
    //    /// <param name="alert_override">   (Optional) The alert override. </param>
    //    ///
    //    /// <returns>   An asynchronous result that yields an IActionResult. </returns>
    //    ////////////////////////////////////////////////////////////////////////////////////////////////////

    //    [HttpPost]
    //    [Route("api/mobile/push/atstore")]
    //    public async Task<IActionResult> AtStores(string device_tokens, Banner banner, string alert_override = null)
    //    {

    //        NotifyRequestPayload request = new NotifyRequestPayload()
    //        {
    //            payload = new PushPayload()
    //            {
    //                alert = alert_override ?? Configuration["Settings:Mobile:PushPayload:Alerts:Alert"],
    //                icon = Configuration["Settings:Mobile:PushPayload:Icon"],
    //                sound = Configuration["Settings:Mobile:PushPayload:Sound"],
    //                vibrate = Configuration["Settings:Mobile:PushPayload:Vibrate"],
    //                title = banner.GetAttribute<BrandAttribute>().Value
    //            },
    //            channel = Configuration["Settings:Mobile:PushPayload:Channels:NearStore"],
    //            to_tokens = device_tokens
    //        };

    //        await engine.NotifyTokensAsync(request, banner);
    //        return Ok();
    //    }

    //    ////////////////////////////////////////////////////////////////////////////////////////////////////
    //    /// <summary>   (An Action that handles HTTP POST requests) weekly ad. </summary>
    //    ///
    //    /// <remarks>   Mcdand, 2/20/2018. </remarks>
    //    ///
    //    /// <param name="memberId">         Identifier for the member. </param>
    //    /// <param name="banner">           The banner. </param>
    //    /// <param name="alert_override">   (Optional) The alert override. </param>
    //    ///
    //    /// <returns>   An asynchronous result. </returns>
    //    ////////////////////////////////////////////////////////////////////////////////////////////////////

    //    [HttpPost]
    //    [Route("api/mobile/push/weekly_ad")]
    //    public async Task WeeklyAd(string memberId, Banner banner, string alert_override = null)
    //    {
    //        string device_tokens = await AzureLoyaltyDatabaseManager.GetDeviceTokensByMemberId(memberId);

    //        NotifyRequestPayload request = new NotifyRequestPayload()
    //        {
    //            payload = new PushPayload()
    //            {
    //                alert = alert_override ?? Configuration["Settings:Mobile:PushPayload:Alerts:WeeklyAd"],
    //                icon = Configuration["Settings:Mobile:PushPayload:Icon"],
    //                sound = Configuration["Settings:Mobile:PushPayload:Sound"],
    //                vibrate = Configuration["Settings:Mobile:PushPayload:Vibrate"],
    //                title = banner.GetAttribute<BrandAttribute>().Value
    //            },
    //            channel = Configuration["Settings:Mobile:PushPayload:Channels:NewWeeklyAd"],
    //            to_tokens = device_tokens
    //        };
    //        await engine.NotifyTokensAsync(request, banner);
    //    }

    //    ////////////////////////////////////////////////////////////////////////////////////////////////////
    //    /// <summary>   (An Action that handles HTTP POST requests) offers expiring. </summary>
    //    ///
    //    /// <remarks>   Mcdand, 2/20/2018. </remarks>
    //    ///
    //    /// <param name="memberId">         Identifier for the member. </param>
    //    /// <param name="banner">           The banner. </param>
    //    /// <param name="alert_override">   (Optional) The alert override. </param>
    //    ///
    //    /// <returns>   An asynchronous result that yields an IActionResult. </returns>
    //    ////////////////////////////////////////////////////////////////////////////////////////////////////

    //    [HttpPost]
    //    [Route("api/mobile/push/offers_expiring")]
    //    public async Task<IActionResult> OffersExpiring(string memberId, Banner banner, string alert_override = null)
    //    {
    //        string device_tokens = await AzureLoyaltyDatabaseManager.GetDeviceTokensByMemberId(memberId);

    //        NotifyRequestPayload request = new NotifyRequestPayload()
    //        {
    //            payload = new PushPayload()
    //            {
    //                alert = alert_override ?? Configuration["Settings:Mobile:PushPayload:Alerts:OffersExpiring"],
    //                icon = Configuration["Settings:Mobile:PushPayload:Icon"],
    //                sound = Configuration["Settings:Mobile:PushPayload:Sound"],
    //                vibrate = Configuration["Settings:Mobile:PushPayload:Vibrate"],
    //                title = banner.GetAttribute<BrandAttribute>().Value
    //            },
    //            channel = Configuration["Settings:Mobile:PushPayload:Channels:OffersExpiring"],
    //            to_tokens = device_tokens
    //        };
    //        await engine.NotifyTokensAsync(request, banner);
    //        return Ok();
    //    }

    //    ////////////////////////////////////////////////////////////////////////////////////////////////////
    //    /// <summary>   (An Action that handles HTTP POST requests) earned points. </summary>
    //    ///
    //    /// <remarks>   Mcdand, 2/20/2018. </remarks>
    //    ///
    //    /// <param name="memberId">         Identifier for the member. </param>
    //    /// <param name="banner">           The banner. </param>
    //    /// <param name="points_earned">    The points earned. </param>
    //    /// <param name="alert_override">   (Optional) The alert override. </param>
    //    ///
    //    /// <returns>   An asynchronous result that yields an IActionResult. </returns>
    //    ////////////////////////////////////////////////////////////////////////////////////////////////////

    //    [HttpPost]
    //    [Route("api/mobile/push/earned_points")]
    //    public async Task<IActionResult> EarnedPoints(string memberId, Banner banner, int points_earned, string alert_override = null)
    //    {
    //        string device_tokens = await AzureLoyaltyDatabaseManager.GetDeviceTokensByMemberId(memberId);

    //        if (!string.IsNullOrWhiteSpace(device_tokens))
    //        {

    //            NotifyRequestPayload request = new NotifyRequestPayload()
    //            {
    //                payload = new PushPayload()
    //                {
    //                    alert = alert_override ?? string.Format(Configuration["Settings:Mobile:PushPayload:Alerts:Earned"], points_earned, banner.GetAttribute<BrandAttribute>().Value),
    //                    icon = Configuration["Settings:Mobile:PushPayload:Icon"],
    //                    sound = Configuration["Settings:Mobile:PushPayload:Sound"],
    //                    vibrate = Configuration["Settings:Mobile:PushPayload:Vibrate"],
    //                    title = banner.GetAttribute<BrandAttribute>().Value
    //                },
    //                channel = Configuration["Settings:Mobile:PushPayload:Channels:PointsEarned"],
    //                to_tokens = device_tokens
    //            };
    //            await engine.NotifyTokensAsync(request, banner);
    //            return Ok();
    //        }
    //        else
    //            return NotFound();
    //    }


    }

}
