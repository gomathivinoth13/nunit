////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	Controllers\MessagingController.cs
//
// summary:	Implements the messaging controller class
////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
//using Twilio;
using SEG.ApiService.Models.Twilio;

using Microsoft.Azure;
using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using SEG.ApiService.Models.Database;
//using Twilio.TwiML.Messaging;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
//using Twilio.Rest.Api.V2010.Account;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SEG.LoyaltyServiceWeb.Controllers
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   A controller for handling messagings. </summary>
    ///
    /// <remarks>   Mcdand, 2/20/2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public class MessagingController : Controller
    {
        ////private static log4net.ILog Logging = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);  ///< The logging
        //IConfiguration Configuration;   ///< The configuration

        //////////////////////////////////////////////////////////////////////////////////////////////////////
        ///// <summary>   Constructor. </summary>
        /////
        ///// <remarks>   Mcdand, 2/20/2018. </remarks>
        /////
        ///// <param name="configuration">    The configuration. </param>
        //////////////////////////////////////////////////////////////////////////////////////////////////////

        //public MessagingController(IConfiguration configuration)
        //{
        //    Configuration = configuration;
        //    Client = new SEG.Twilio.Client(Configuration["Settings:Twilio:AccountSid"], Configuration["Settings:Twilio:AuthToken"], Configuration["Settings:Twilio:MessageServiceSid"]);

        //}

        //////////////////////////////////////////////////////////////////////////////////////////////////////
        ///// <summary>   Gets or sets the client. </summary>
        /////
        ///// <value> The client. </value>
        //////////////////////////////////////////////////////////////////////////////////////////////////////

        //internal SEG.Twilio.Client Client { get; private set; }

        //////////////////////////////////////////////////////////////////////////////////////////////////////
        ///// <summary>   (An Action that handles HTTP POST requests) sends a message. </summary>
        /////
        ///// <remarks>   Mcdand, 2/20/2018. </remarks>
        /////
        ///// <param name="sendMessageRequest">   The send message request. </param>
        ///// <param name="apiKey">   Api Key for Sending SMS Messages </param>
        ///// 
        //////////////////////////////////////////////////////////////////////////////////////////////////////

        //[HttpPost]
        //[Route("api/Messaging/SendMessage")]
        //public async Task<IActionResult> SendMessage([FromBody]SendMessageRequest sendMessageRequest )
        //{
 
        //    await Client.SendMessageAsync(sendMessageRequest.PhoneNumber, sendMessageRequest.Body).ConfigureAwait(false);
        //    return Ok();
        //}

    }
}