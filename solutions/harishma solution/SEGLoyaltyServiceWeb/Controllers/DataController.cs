////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	Controllers\DataController.cs
//
// summary:	Implements the data controller class
///////////////////////////////////////////////////////////////////////////////////////////////////////

using Microsoft.AspNetCore.Mvc;
using SEG.ApiService.Models.SilverPop;
using SEG.AzureLoyaltyDatabase;
using SEG.AzureLoyaltyDatabase.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
 
namespace SEG.LoyaltyServiceWeb.Controllers
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   A controller for handling data. </summary>
    ///
    /// <remarks>   Mcdand, 2/20/2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public class DataController :Controller
    {
    //    #region Push Notifications

    //    ////////////////////////////////////////////////////////////////////////////////////////////////////
    //    /// <summary>
    //    /// (An Action that handles HTTP PUT requests) adds a push subscribe request.
    //    /// </summary>
    //    ///
    //    /// <remarks>   Mcdand, 2/20/2018. </remarks>
    //    ///
    //    /// <param name="deviceId">     Identifier for the device. </param>
    //    /// <param name="deviceToken">  The device token. </param>
    //    /// <param name="type">         The type. </param>
    //    /// <param name="channel">      The channel. </param>
    //    ///
    //    /// <returns>   An asynchronous result that yields an IActionResult. </returns>
    //    ////////////////////////////////////////////////////////////////////////////////////////////////////

    //    [HttpPut]
    //    public async Task<IActionResult> AddPushSubscribeRequest(string deviceId, string deviceToken, string type, string channel)
    //    {
    //        if (await AzureLoyaltyDatabaseManager.AddPushSubscribeRequest(deviceId, deviceToken, type, channel).ConfigureAwait(false))
    //            return Ok();
    //        else
    //            return BadRequest();
    //    }

    //    ////////////////////////////////////////////////////////////////////////////////////////////////////
    //    /// <summary>
    //    /// (An Action that handles HTTP DELETE requests) deletes the push subscribe channel request.
    //    /// </summary>
    //    ///
    //    /// <remarks>   Mcdand, 2/20/2018. </remarks>
    //    ///
    //    /// <param name="deviceId"> Identifier for the device. </param>
    //    /// <param name="channel">  The channel. </param>
    //    ///
    //    /// <returns>   An asynchronous result that yields an IActionResult. </returns>
    //    ////////////////////////////////////////////////////////////////////////////////////////////////////

    //    [HttpDelete]
    //    public async Task<IActionResult> DeletePushSubscribeChannelRequest(string deviceId, string channel)
    //    {
    //        if (await AzureLoyaltyDatabaseManager.DeletePushSubscribeChannelRequest(deviceId, channel).ConfigureAwait(false))
    //            return Ok();
    //        else
    //            return BadRequest();
    //    }
    //    #endregion

    //    #region Silverpop

    //    ////////////////////////////////////////////////////////////////////////////////////////////////////
    //    /// <summary>
    //    /// (An Action that handles HTTP POST requests) updates the silver pop access token.
    //    /// </summary>
    //    ///
    //    /// <remarks>   Mcdand, 2/20/2018. </remarks>
    //    ///
    //    /// <param name="silverPopAccessToken"> The silver pop access token. </param>
    //    /// <param name="appCode">              The application code. </param>
    //    ///
    //    /// <returns>   An asynchronous result that yields an IActionResult. </returns>
    //    ////////////////////////////////////////////////////////////////////////////////////////////////////

    //    [HttpPost]
    //    public async Task<IActionResult> UpdateSilverPopAccessToken([FromBody]ApiService.Models.SilverPop.SilverPopAccessToken silverPopAccessToken, string appCode)
    //    {
    //        await SilverpopAccessTokenDAL.SaveSilverPopAccessToken(silverPopAccessToken);
    //        return Ok();
    //    }

    //    ////////////////////////////////////////////////////////////////////////////////////////////////////
    //    /// <summary>
    //    /// (An Action that handles HTTP GET requests) gets silver pop access token.
    //    /// </summary>
    //    ///
    //    /// <remarks>   Mcdand, 2/20/2018. </remarks>
    //    ///
    //    /// <param name="appCode">  The application code. </param>
    //    ///
    //    /// <returns>   An asynchronous result that yields the silver pop access token. </returns>
    //    ////////////////////////////////////////////////////////////////////////////////////////////////////

    //    [HttpGet]
    //    public async Task<SilverPopAccessToken> GetSilverPopAccessToken(string appCode)
    //    {
    //        return await SilverpopAccessTokenDAL.GetAccessToken();
    //    }

    //    #endregion

    //    #region OfferQueue

    //    ////////////////////////////////////////////////////////////////////////////////////////////////////
    //    /// <summary>   Updates the coupon identifier. </summary>
    //    ///
    //    /// <remarks>   Mcdand, 2/20/2018. </remarks>
    //    ///
    //    /// <param name="memberId">     Identifier for the member. </param>
    //    /// <param name="chainId">      Identifier for the chain. </param>
    //    /// <param name="couponId">     Identifier for the coupon. </param>
    //    /// <param name="couponAlias">  The coupon alias. </param>
    //    ///
    //    /// <returns>   An IActionResult. </returns>
    //    ////////////////////////////////////////////////////////////////////////////////////////////////////

    //    public IActionResult UpdateCouponId(string memberId, string chainId, string couponId, string couponAlias)
    //    {
    //        AzureLoyaltyDatabaseManager.UpdateCouponId(memberId, chainId, couponId, couponAlias);
    //        return Ok();
    //    }
    //    #endregion

    }
}
