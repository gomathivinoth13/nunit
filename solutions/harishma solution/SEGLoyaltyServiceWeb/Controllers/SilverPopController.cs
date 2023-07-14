////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	Controllers\SilverPopController.cs
//
// summary:	Implements the silver pop controller class
////////////////////////////////////////////////////////////////////////////////////////////////////

using SEG.ApiService.Models.Offers.Request;
using SEG.ApiService.Models.SilverPop;
using SEG.ApiService.Models.SilverPop.Request;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SalesForceLibrary.Models;
using SalesForceLibrary.Queue;
using Microsoft.Extensions.Caching.Distributed;

namespace SEG.LoyaltyServiceWeb.Controllers
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   A controller for handling silver pops. </summary>
    ///
    /// <remarks>   Mcdand, 2/20/2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public class SilverPopController : Controller
    {
        //SEG.SilverPopLibrary.SilverPop service; ///< The service

        //IConfiguration Configuration;   ///< The configuration

        //////////////////////////////////////////////////////////////////////////////////////////////////////
        ///// <summary>   Constructor. </summary>
        /////
        ///// <remarks>   Mcdand, 2/20/2018. </remarks>
        /////
        ///// <param name="configuration">    The configuration. </param>
        //////////////////////////////////////////////////////////////////////////////////////////////////////

        //public SilverPopController(IConfiguration configuration)
        //{

        //    Configuration = configuration;

        //    service = new SEG.SilverPopLibrary.SilverPop(
        //                                Configuration["Settings:SilverPop:EndPoint"],
        //                                   Configuration["Settings:SilverPop:ClientId"],
        //                                  Configuration["Settings:SilverPop:ClientSecret"],
        //                                  Configuration["Settings:SilverPop:ListId"],
        //                                  Configuration["Settings:SilverPop:RefreshToken"],
        //                                  Configuration["Settings:StorageConnectionString"],
        //                                  Configuration["Settings:AzureLoyaltyApiEndpoint"],
        //                                 Configuration["Settings:CacheConnectionString"]
        //                                   );
        //}

        //////////////////////////////////////////////////////////////////////////////////////////////////////
        ///// <summary>   (An Action that handles HTTP POST requests) adds a recipient. </summary>
        /////
        ///// <remarks>   Mcdand, 2/20/2018. </remarks>
        /////
        ///// <param name="silverPopAddRecipientRequest"> The silver pop add recipient request. </param>
        /////
        ///// <returns>   A SilverPopSuccessFailureResponse. </returns>
        //////////////////////////////////////////////////////////////////////////////////////////////////////

        //[HttpPost]
        //[Route("api/SilverPop/AddRecipient")]
        //public SilverPopSuccessFailureResponse AddRecipient([FromBody]SilverPopAddRecipientRequest silverPopAddRecipientRequest)
        //{
        //    return service.AddRecipient(silverPopAddRecipientRequest.transactionID, silverPopAddRecipientRequest.appCode, silverPopAddRecipientRequest.appVer, silverPopAddRecipientRequest.addRecipientRequest);
        //}

        //////////////////////////////////////////////////////////////////////////////////////////////////////
        ///// <summary>   (An Action that handles HTTP POST requests) gets access token. </summary>
        /////
        ///// <remarks>   Mcdand, 2/20/2018. </remarks>
        /////
        ///// <param name="getAccessTokenRequest">    The get access token request. </param>
        /////
        ///// <returns>   The access token. </returns>
        //////////////////////////////////////////////////////////////////////////////////////////////////////

        //[HttpPost]
        //[Route("api/SilverPop/GetAccessToken")]
        //public SilverPopAccessToken GetAccessToken([FromBody]GetAccessTokenRequest getAccessTokenRequest)
        //{
        //    return service.GetAccessToken(getAccessTokenRequest.transactionID, getAccessTokenRequest.appCode, getAccessTokenRequest.appVer);
        //}

        //////////////////////////////////////////////////////////////////////////////////////////////////////
        ///// <summary>
        ///// (An Action that handles HTTP POST requests) removes the recipient described by
        ///// silverPopRemoveRecipientRequest.
        ///// </summary>
        /////
        ///// <remarks>   Mcdand, 2/20/2018. </remarks>
        /////
        ///// <param name="silverPopRemoveRecipientRequest">  The silver pop remove recipient request. </param>
        /////
        ///// <returns>   A SilverPopSuccessFailureResponse. </returns>
        //////////////////////////////////////////////////////////////////////////////////////////////////////

        //[HttpPost]
        //[Route("api/SilverPop/RemoveRecipient")]
        //public SilverPopSuccessFailureResponse RemoveRecipient([FromBody]SilverPopRemoveRecipientRequest silverPopRemoveRecipientRequest)
        //{
        //    return service.RemoveRecipient(silverPopRemoveRecipientRequest.transactionID, silverPopRemoveRecipientRequest.appVer, silverPopRemoveRecipientRequest.appVer, silverPopRemoveRecipientRequest.removeRecipientRequest);
        //}

        //////////////////////////////////////////////////////////////////////////////////////////////////////
        ///// <summary>   (An Action that handles HTTP POST requests) select recipient. </summary>
        /////
        ///// <remarks>   Mcdand, 2/20/2018. </remarks>
        /////
        ///// <param name="silverPopSelectRecipientRequest">  The silver pop select recipient request. </param>
        /////
        ///// <returns>   A SilverPopSuccessFailureResponse. </returns>
        //////////////////////////////////////////////////////////////////////////////////////////////////////

        //[HttpPost]
        //[Route("api/SilverPop/SelectRecipient")]
        //public SilverPopSuccessFailureResponse SelectRecipient([FromBody]SilverPopSelectRecipientRequest silverPopSelectRecipientRequest)
        //{
        //    return service.SelectRecipient(silverPopSelectRecipientRequest.transactionID, silverPopSelectRecipientRequest.appCode, silverPopSelectRecipientRequest.appVer, silverPopSelectRecipientRequest.selectRecipientRequest);
        //}

        //////////////////////////////////////////////////////////////////////////////////////////////////////
        ///// <summary>
        ///// (An Action that handles HTTP POST requests) updates the recipient described by
        ///// silverPopUpdateRecipientRequest.
        ///// </summary>
        /////
        ///// <remarks>   Mcdand, 2/20/2018. </remarks>
        /////
        ///// <param name="silverPopUpdateRecipientRequest">  The silver pop update recipient request. </param>
        /////
        ///// <returns>   A SilverPopSuccessFailureResponse. </returns>
        //////////////////////////////////////////////////////////////////////////////////////////////////////

        //[HttpPost]
        //[Route("api/SilverPop/UpdateRecipient")]
        //public SilverPopSuccessFailureResponse UpdateRecipient([FromBody]SilverPopUpdateRecipientRequest silverPopUpdateRecipientRequest)
        //{
        //    return service.UpdateRecipient(silverPopUpdateRecipientRequest.transactionID, silverPopUpdateRecipientRequest.appCode, silverPopUpdateRecipientRequest.appVer, silverPopUpdateRecipientRequest.updateRecipientRequest);
        //}

        //////////////////////////////////////////////////////////////////////////////////////////////////////
        ///// <summary>
        ///// (An Action that handles HTTP POST requests) inserts a silver pop queue described by
        ///// silverPopQueueRequest.
        ///// </summary>
        /////
        ///// <remarks>   Mcdand, 2/20/2018. </remarks>
        /////
        ///// <param name="silverPopQueueRequest">    The silver pop queue request. </param>
        /////
        ///// <returns>   An asynchronous result that yields an IActionResult. </returns>
        //////////////////////////////////////////////////////////////////////////////////////////////////////

        //[HttpPost]
        //[Route("api/SilverPop/InsertSilverPopQueue")]
        //public async Task<IActionResult> InsertSilverPopQueue([FromBody]SilverPopQueueRequest silverPopQueueRequest)
        //{
        //    await service.InsertSilverPopQueue(silverPopQueueRequest).ConfigureAwait(false);
        //    return Ok();
        //}


    }
}
