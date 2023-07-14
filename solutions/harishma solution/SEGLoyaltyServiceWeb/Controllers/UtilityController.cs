
using Microsoft.AspNetCore.Mvc;
using SEG.ApiService.Models.Twilio;
using SEG.AzureLoyaltyDatabase;
using SEG.LoyaltyDatabase.Models;
//using SEG.Twilio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SEG.ApiService.Models.Payload;
using Microsoft.AspNetCore.Hosting;
using SEG.CustomerLibrary.Process;
using SEG.ApiService.Models.Offers;

namespace SEGLoyaltyServiceWeb.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class UtilityController : Controller
    {

        //private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);  ///< The log
        IConfiguration Configuration;
        //Client twilioClient;
        //SEG.Twilio.Utility.SEGPhoneNumberValidation segValidation;

        private readonly IHostingEnvironment _hostingEnvironment;



        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public UtilityController(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            Configuration = configuration;
            //twilioClient = new Client(Configuration["Settings:Twilio:AccountSid"], Configuration["Settings:Twilio:AuthToken"], Configuration["Settings:Twilio:MessageServiceSid"]);
            //segValidation = new SEG.Twilio.Utility.SEGPhoneNumberValidation(twilioClient, Configuration["Settings:PhoneNumberConnection"]);
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="phoneNumber"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("api/Utility/PhoneLookUp")]
        //[Produces(typeof(CustPhoneLookup))]
        //public async Task<IActionResult> PhoneLookUp(string phoneNumber)
        //{
        //    //Carrier carrier = null;
        //    CustPhoneLookup custPhoneLookup = null;

        //    try
        //    {
        //        if (!string.IsNullOrEmpty(phoneNumber))
        //        {

        //            custPhoneLookup = await segValidation.ValidatePhoneNumber(phoneNumber);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //log.Error($"Error getting Response message for PhoneLookUp:" + phoneNumber, ex);
        //        return BadRequest($"Error getting Response message for PhoneLookUp: {phoneNumber} [{ex.Message}]");
        //    }

        //    return Ok(custPhoneLookup);
        //}

        //[HttpPost]
        //[Route("api/Utility/GenerateApplePass")]
        //public string GenerateApplePass(string barcodeNumber)
        //{
        //    try
        //    {
        //        return Convert.ToBase64String(PassKit.GeneratePass(barcodeNumber, _hostingEnvironment.ContentRootPath, _hostingEnvironment.IsDevelopment(), Configuration["AppleWWDRCAThumbprint"], Configuration["SEGCertificateThumbprint"]));
        //    }
        //    catch (Exception e)
        //    {

        //        //log.Error("Error Generationg Apple PKPass", e);
        //        return e.ToString();
        //    }

        //}

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>  Gets SEG categories. </summary>
        ///
        /// <remarks>  Mark Robinson 7/28/22020. </remarks>
        ///
        /// <returns>
        /// An asynchronous result that yields categories.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        //[HttpGet]
        //[Route("api/Utility/GetCategories")]
        //public async Task<List<string>> GetCategories()
        //{
        //    return await AzureLoyaltyDatabaseManager.GetCategories().ConfigureAwait(false);
        //}


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Utility/GetEnrollmentOfferEE")]
        [Produces(typeof(string))]
        public async Task<IActionResult> GetEnrollmentOfferEE(string chainId)
        {
            try
            {
                OfferProcess offersProcess = new OfferProcess();

                if (string.IsNullOrEmpty(chainId))
                {
                    OfferFailureResponse error = new OfferFailureResponse()
                    {
                        ErrorCode = "400 Bad Request",
                        ErrorDescription = "Request cannot be null , ChainID is required "
                    };

                    return StatusCode(400, error);
                }

                string campaignId = await offersProcess.GetEnrollmentOfferEE(chainId).ConfigureAwait(false);

                return StatusCode((int)200, campaignId);
            }
            catch (Exception ex)
            {
                throw new Exception("Exception in GetEnrollmentOfferEE :", ex);
            }
        }
    }
}
