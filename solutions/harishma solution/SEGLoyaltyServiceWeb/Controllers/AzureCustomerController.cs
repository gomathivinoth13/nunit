////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	Controllers\AzureCustomerController.cs
//
// summary:	Implements the azure customer controller class
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
    /// <summary>   A controller for handling azure customers. </summary>
    ///
    /// <remarks>   Mcdand, 2/20/2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public class AzureCustomerController : Controller
    {
        //private Microsoft.ApplicationInsights.TelemetryClient telemetry = new Microsoft.ApplicationInsights.TelemetryClient();
        ////private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);  ///< The log

        //static SEG.CustomerLibrary.CustomerService customerServiceAzure;    ///< The customer service azure
        //OfferService offerService;  ///< The offer service
        //IConfiguration Configuration;   ///< The configuration

        //////////////////////////////////////////////////////////////////////////////////////////////////////
        ///// <summary>   Constructor. </summary>
        /////
        ///// <remarks>   Mcdand, 2/20/2018. </remarks>
        /////
        ///// <param name="configuration">    The configuration. </param>
        //////////////////////////////////////////////////////////////////////////////////////////////////////

        //public AzureCustomerController(IConfiguration configuration)
        //{
        //    Configuration = configuration;
        //    offerService = new OfferService(Configuration["Settings:StorageConnectionString"]);
        //    customerServiceAzure = new SEG.CustomerLibrary.CustomerService(
        //      Configuration["Settings:StoreWebAPIEndPoint"],
        //      Configuration["Settings:OfferWebAPIEndPoint"],
        //      Configuration["Settings:LoyaltyEndPoint"],

        //      Configuration["Settings:AzureLoyaltyApiEndpoint"],
        //      Configuration["Settings:StorageConnectionString"],

        //      Configuration["Settings:CacheConnectionString"],
        //       Configuration["Settings:SalesForce:SalesForceAPIEndPoint"]);
        //}

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// (An Action that handles HTTP POST requests) migrates the given migrate user request.
        /// </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="migrateUserRequest">   The migrate user request. </param>
        ///
        /// <returns>   An asynchronous result that yields an IActionResult. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        //[Route("api/AzureCustomer/Migrate")]
        //[HttpPost]
        //public async Task<IActionResult> Migrate([FromBody]MigrateUserRequest migrateUserRequest)
        //{
        //    //migrateUserRequest = await GetObjectFromPostDataIfNull<MigrateUserRequest>(migrateUserRequest).ConfigureAwait(false);

        //    bool success = false;
        //    try
        //    {
        //        success = await customerServiceAzure.Migrate(migrateUserRequest.DeviceId, migrateUserRequest.TransactionID, migrateUserRequest.EmailAddress, migrateUserRequest.AppCode, migrateUserRequest.AppVer, migrateUserRequest.StoreId, migrateUserRequest.ShoppingList).ConfigureAwait(false);
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(String.Format("An error while running Migrate.  Error {0}", ex.Message), ex);
        //        return BadRequest(ex.ToString());
        //    }
        //    return Ok(success);
        //}

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   (An Action that handles HTTP POST requests) generates a barcode. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="value">    The value. </param>
        /// <param name="size">     The size. </param>
        ///
        /// <returns>   An asynchronous result that yields the barcode. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        //[HttpPost]
        //[Route("api/AzureCustomer/GenerateBarcode")]
        //public async Task<BarcodeResponse> GenerateBarcode(string value, Shared.Barcodes.BarcodeSize size)
        //{
        //    try
        //    {
        //        return await Task.Run(() =>
        //           {

        //               var barcode = new BarcodeResponse()
        //               {
        //                   BarcodeValue = value,
        //                   ImageBase64 = Shared.BarcodeGenerator.GenerateBarcodeBase64(value, ZXing.BarcodeFormat.CODE_128, size)
        //               };
        //               barcode.Size = (int)size;
        //               return barcode;
        //           }).ConfigureAwait(false);
        //    }
        //    finally
        //    {
        //        telemetry.TrackEvent("Customer_DeleteAttribute");
        //    }
        //}



    }
}