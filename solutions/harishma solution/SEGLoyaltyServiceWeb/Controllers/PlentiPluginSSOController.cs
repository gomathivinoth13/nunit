////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	Controllers\PlentiPluginSSOController.cs
//
// summary:	Implements the plenti plugin sso controller class
////////////////////////////////////////////////////////////////////////////////////////////////////

using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using SEG.ApiService.Models;
using SEG.ApiService.Models.Database;
using SEG.ApiService.Models.Enum;
using SEG.ApiService.Models.Request;
using SEG.LoyaltyService.Models.Results;
using SEG.ApiService.Models.Mobile;
using SEG.CustomerLibrary.Process;
using SEG.ApiService.Models.Offers;
using SEG.OffersLibrary;
using SEG.ApiService.Models.Offers.Request;
using System.Collections.Generic;
using System.Linq;
using SEG.ApiService.Models.Attributes;
using System.IO.Compression;
using System.Net;
using System.Configuration;
using SEGShared = SEG.Shared;
using System.Net.Http;
using Microsoft.Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace SEG.LoyaltyServiceWeb.Controllers
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   A controller for handling plenti plugin ssoes. </summary>
    ///
    /// <remarks>   Mcdand, 2/20/2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public class PlentiPluginSSOController : Controller
    {
        //SEG.CustomerLibrary.CustomerService customerServiceAzure;   ///< The customer service azure
        //IConfiguration Configuration;   ///< The configuration

        //////////////////////////////////////////////////////////////////////////////////////////////////////
        ///// <summary>   Constructor. </summary>
        /////
        ///// <remarks>   Mcdand, 2/20/2018. </remarks>
        /////
        ///// <param name="configuration">    The configuration. </param>
        //////////////////////////////////////////////////////////////////////////////////////////////////////

        //public PlentiPluginSSOController(IConfiguration configuration)
        //{
        //    Configuration = configuration;
        //    customerServiceAzure = new SEG.CustomerLibrary.CustomerService(Configuration["Settings:StoreWebAPIEndPoint"], Configuration["Settings:OFferWebAPIEndPoint"], Configuration["Settings:LoyaltyEndPoint"], Configuration["Settings:AzureLoyaltyApiEndpoint"], Configuration["Settings:StorageConnectionString"], Configuration["Settings:CacheConnectionString"], Configuration["Settings:SalesForce:SalesForceAPIEndPoint"]);
        //}
        //#region Plenti SSO 

        //////////////////////////////////////////////////////////////////////////////////////////////////////
        ///// <summary>   Gets URL of the sso. </summary>
        /////
        ///// <value> The sso URL. </value>
        //////////////////////////////////////////////////////////////////////////////////////////////////////

        //public string SSOUrl { get { return Configuration["Settings:Plenti:SsoUrl"]; } }

        //////////////////////////////////////////////////////////////////////////////////////////////////////
        ///// <summary>   (An Action that handles HTTP POST requests) gets access code. </summary>
        /////
        ///// <remarks>   Mcdand, 2/20/2018. </remarks>
        /////
        ///// <param name="token">    The token. </param>
        ///// <param name="banner">   The banner. </param>
        /////
        ///// <returns>   An asynchronous result that yields the access code. </returns>
        //////////////////////////////////////////////////////////////////////////////////////////////////////

        ////[HttpPost]
        ////[Route("api/PlentiPluginSSO/GetAccessCode")]
        ////public async Task<OmniSSOGetAccessCodeResponse> GetAccessCode(string token, Banner banner)
        ////{
        ////    //HttpWebRequest httpRequest = null;
        ////    //string serviceResponse;
        ////    try
        //    {
        //        string _SSOPrincipalAttribute = banner.GetAttribute<SSOPrincipalAttribute>().Value;
        //        //string ssoUrl = SSOUrl + _SSOPrincipalAttribute + "/math/v2/access_codes/";
        //        string pathSegments = _SSOPrincipalAttribute + "/math/v2/access_codes/";
        //        //httpRequest = (HttpWebRequest)WebRequest.Create(ssoUrl);
        //        //httpRequest.Proxy = null;
        //        //httpRequest.KeepAlive = false;
        //        //httpRequest.ContentType = "application/json";
        //        //httpRequest.Method = "POST";
        //        //httpRequest.Timeout = 1000 * 30;


        //        var credentials = new NetworkCredential(_SSOPrincipalAttribute, Configuration[$"Settings:Plenti:Credentials:{ _SSOPrincipalAttribute}"].DecryptStringAES());
        //        //using (var writer = new StreamWriter(httpRequest.GetRequestStream()))
        //        //{
        //        //    writer.Write(JsonConvert.SerializeObject(new { token }));
        //        //    writer.Close();
        //        //}
        //        //using (var response = (HttpWebResponse)httpRequest.GetResponse())
        //        //{
        //        //    using (var responseStream = GetStreamForResponse(response))
        //        //    {
        //        //        using (StreamReader streamReader = new StreamReader(responseStream))
        //        //        {
        //        //            serviceResponse = streamReader.ReadToEnd();
        //        //            streamReader.Close();
        //        //        }
        //        //        responseStream.Close();
        //        //    }
        //        //    response.Close();
        //        //}
        //        //OmniSSOGetAccessCodeResponse omniSSOGetAccessCodeResponse = JsonConvert.DeserializeObject<OmniSSOGetAccessCodeResponse>(serviceResponse);

        //        OmniSSOGetAccessCodeResponse omniSSOGetAccessCodeResponse = (await SEG.Shared.ApiUtility.RestfulCallAsync<OmniSSOGetAccessCodeResponse>(HttpMethod.Post, new { token }, pathSegments, SSOUrl, Authentication: credentials).ConfigureAwait(false)).Result;
        //        return omniSSOGetAccessCodeResponse;
        //    }
        //    catch (Exception ex)
        //    { throw; }
        //    //finally
        //    //{
        //    //    if (httpRequest != null)
        //    //        httpRequest = null;
        //    //}
        //}

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets stream for response. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="webResponse">  The web response. </param>
        /// <param name="readTimeOut">  (Optional) The read time out. </param>
        ///
        /// <returns>   The stream for response. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

//        private static Stream GetStreamForResponse(HttpWebResponse webResponse, int readTimeOut = 90)
//        {
//            Stream stream;
//            switch (webResponse.ContentEncoding.ToUpperInvariant())
//            {
//                case "GZIP":
//                    stream = new GZipStream(webResponse.GetResponseStream(), CompressionMode.Decompress);
//                    break;
//                case "DEFLATE":
//                    stream = new DeflateStream(webResponse.GetResponseStream(), CompressionMode.Decompress);
//                    break;
//                default:
//                    stream = webResponse.GetResponseStream();
//                    stream.ReadTimeout = readTimeOut;
//                    break;
//            }
//            return stream;
//        }
//        #endregion

//        ////////////////////////////////////////////////////////////////////////////////////////////////////
//        /// <summary>   refresh access token. </summary>
//        ///
//        /// <remarks>   Mcdand, 2/20/2018. </remarks>
//        ///
//        /// <exception cref="ArgumentException">        Thrown when one or more arguments have
//        ///                                             unsupported or illegal values. </exception>
//        /// <exception cref="HttpResponseException">    Thrown when a HTTP Response error condition
//        ///                                             occurs. </exception>
//        ///
//        /// <param name="refreshTokenRequest">  . </param>
//        ///
//        /// <returns>   An asynchronous result that yields an AccessTokenIndex. </returns>
//        ////////////////////////////////////////////////////////////////////////////////////////////////////

////        [HttpPost]
////        [Route("api/Customer/RefreshToken")]
////        public async Task<AccessTokenIndex> RefreshToken([FromBody]RefreshTokenRequest refreshTokenRequest)
////        {
////            try
////            {

////                //Akana is not posting the object correctly??? 

////                if (refreshTokenRequest == null) throw new ArgumentException($"Missing RefreshTokenRequest");


////                AccessTokenIndex accessTokenIndex = new AccessTokenIndex();
////                SEGShared.Utility utility = new SEGShared.Utility();
////                string chainId = utility.SetChainId(refreshTokenRequest.appCode);
////                OmniSSOGetAccessCodeResponse omniSSOGetAccessCodeResponse = null;
////                accessTokenIndex = await customerServiceAzure.RefreshToken(refreshTokenRequest.transactionID, refreshTokenRequest.appCode, refreshTokenRequest.appVer, refreshTokenRequest.emailAddress, refreshTokenRequest.accessToken, refreshTokenRequest.newAccessToken, refreshTokenRequest.deviceId).ConfigureAwait(false);
////                if (accessTokenIndex == null) throw new ArgumentException("Refresh Token Did not return a result");
////                omniSSOGetAccessCodeResponse = await GetAccessCode(refreshTokenRequest.newAccessToken, (Banner)int.Parse(chainId)).ConfigureAwait(false);
////                if (omniSSOGetAccessCodeResponse != null)
////                    accessTokenIndex.access_code = omniSSOGetAccessCodeResponse.access_code;
////                return accessTokenIndex;

////            }
////            catch (Exception e)
////            {
////                if (e.ToString().Contains("LOY-"))
////#if DEBUG
////                    throw new HttpResponseException(new HttpResponseMessage() { Content = new StringContent(e.ToString()), ReasonPhrase = e.Message, StatusCode = HttpStatusCode.BadRequest });
////#else
////                    throw new HttpResponseException(new HttpResponseMessage() { Content = new StringContent(e.Message), ReasonPhrase = e.Message, StatusCode = HttpStatusCode.BadRequest });
////#endif
////                else
////                    throw;
////            }
////        }

    }
}
