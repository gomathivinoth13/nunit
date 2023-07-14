using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SEG.ApiService.Models;
using SEG.ApiService.Models.Queueing;
using SEG.CustomerLibrary.Process;
using SEG.AzureLoyaltyDatabase;
using SEG.CustomerLibrary;

namespace SEGLoyaltyServiceWeb.Controllers
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   A controller for handling pushes. </summary>
    ///
    /// <remarks>   Mcdand, 2/28/2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    [Produces("application/json")]
    [Route("api/notifications")]
    public class XPushController : Controller
    {
        //readonly IConfiguration Configuration;
        //SEG.ApiService.Models.Utility.Utilities Utilities;

        //////////////////////////////////////////////////////////////////////////////////////////////////////
        ///// <summary>   Constructor. </summary>
        /////
        ///// <remarks>   Mcdand, 4/30/2018. </remarks>
        /////
        ///// <param name="configuration">    The configuration. </param>
        //////////////////////////////////////////////////////////////////////////////////////////////////////

        //public XPushController(IConfiguration configuration)
        //{
        //    Configuration = configuration;
        //    Utilities = new SEG.ApiService.Models.Utility.Utilities(Configuration["Settings:StorageConnectionString"]);

        //}

        //////////////////////////////////////////////////////////////////////////////////////////////////////
        ///// <summary>   Registers The device and Firebase Access Token combination. </summary>
        /////
        ///// <remarks>   Mcdand, 2/28/2018. </remarks>
        /////
        ///// <param name="device">   Device Object </param>
        ///// <param name="appId">    Application ID </param>
        /////
        ///// <returns>   An asynchronous result that yields an IActionResult. </returns>
        /////
        //////////////////////////////////////////////////////////////////////////////////////////////////////

        //[HttpPost]
        //[Route("register")]
        //public async Task<IActionResult> Register([FromBody]SEG.ApiService.Models.Database.Device device, string appId)
        //{
        //    device.Chain = CustomerService.GetChainCodeFromAppId(appId);
        //    device.LastLoginDateTime = DateTime.Now;

        //    await AzureLoyaltyDatabaseManager.SaveDevice(device);

        //    return Ok();
        //}

        //////////////////////////////////////////////////////////////////////////////////////////////////////
        ///// <summary>   Queue Push Notification(s) </summary>
        /////
        ///// <remarks>   Mcdand, 4/30/2018. </remarks>
        /////
        ///// <param name="banner">           The banner. </param>
        ///// <param name="MemberID">         (Optional) Identifier for the member. </param>
        ///// <param name="topic">            (Optional) The topic. </param>
        ///// <param name="overridemessage">  (Optional) The overridemessage. </param>
        /////
        ///// <returns>   An asynchronous result that yields an IActionResult. </returns>
        //////////////////////////////////////////////////////////////////////////////////////////////////////
        //[HttpPost]
        //[Route("queuemessage")]
        //public async Task<IActionResult> QueueNotification(Banner banner, SEG.ApiService.Models.Mobile.Topics topic, string MemberID = null, string overridemessage = null)
        //{
        //    var messageDetails = new Dictionary<string, object>() {
        //        { "banner",banner },
        //        { "member_id", MemberID },
        //        { "topic", topic},
        //        { "override_message", overridemessage }
        //    };


        //    QueueTask queueTask = new QueueTask()
        //    {
        //        MethodName = "PushNotification",
        //        QueueName = QueueNameType.PushNotifications,
        //        QueueObject = messageDetails,
        //        ContinueOnError = true,
        //        IsQueueRequest = true,
        //    };

        //    if (await Utilities.AddQueueTaskAsync(queueTask))
        //        return Ok();
        //    else
        //        return BadRequest();
        //}
    }
}