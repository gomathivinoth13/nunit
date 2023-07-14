////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	Controllers\LocatorController.cs
//
// summary:	Implements the locator controller class
////////////////////////////////////////////////////////////////////////////////////////////////////

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SEG.ApiService.Models.Stores;
using SEG.ApiService.Models.Stores.Request;
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
    /// <summary>   A controller for handling locators. </summary>
    ///
    /// <remarks>   Mcdand, 2/20/2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public class LocatorController : Controller
    {
    //    SEG.StoreLibrary.Controllers.StoreWebApiDAL service;    ///< The service

    //    IConfiguration Configuration;   ///< The configuration

    //    ////////////////////////////////////////////////////////////////////////////////////////////////////
    //    /// <summary>   Constructor. </summary>
    //    ///
    //    /// <remarks>   Mcdand, 2/20/2018. </remarks>
    //    ///
    //    /// <param name="configuration">    The configuration. </param>
    //    ////////////////////////////////////////////////////////////////////////////////////////////////////

    //    public LocatorController(IConfiguration configuration)
    //    {
    //        Configuration = configuration;
    //        service = new StoreLibrary.Controllers.StoreWebApiDAL(Configuration["Settings:StoreWebAPIEndPoint"]);
    //    }

    //    ////////////////////////////////////////////////////////////////////////////////////////////////////
    //    /// <summary>   (An Action that handles HTTP POST requests) gets store locations. </summary>
    //    ///
    //    /// <remarks>   Mcdand, 2/20/2018. </remarks>
    //    ///
    //    /// <param name="getStoreLocationsRequest"> The get store locations request. </param>
    //    ///
    //    /// <returns>   An asynchronous result that yields the store locations. </returns>
    //    ////////////////////////////////////////////////////////////////////////////////////////////////////

    //    //[HttpPost]
    //    //[Route("api/mobile/locator/getStores")]
    //    //public async Task<List<Store>> GetStoreLocations([FromBody]GetStoreLocationsRequest getStoreLocationsRequest)
    //    //{
    //    //    return await  service.GetStoreLocationsAsync(getStoreLocationsRequest);
    //    //}

    //    ////////////////////////////////////////////////////////////////////////////////////////////////////
    //    /// <summary>   (An Action that handles HTTP POST requests) gets closest stores. </summary>
    //    ///
    //    /// <remarks>   Mcdand, 2/20/2018. </remarks>
    //    ///
    //    /// <param name="getStoreLocationsRequest"> The get store locations request. </param>
    //    ///
    //    /// <returns>   An asynchronous result that yields the closest stores. </returns>
    //    ////////////////////////////////////////////////////////////////////////////////////////////////////

    //    //[HttpPost]
    //    //[Route("api/mobile/locator/getClosestStores")]
    //    //public async Task< List<Store>> GetClosestStores([FromBody]GetClosestStoresRequest getStoreLocationsRequest)
    //    //{
    //    //    bool.TryParse(Configuration["Settings:Mobile:DisableGeoFencing"], out bool disableGeoFencing);

    //    //    if (disableGeoFencing)
    //    //        return new List<Store>();
    //    //    else
    //    //        return await service.GetClosestStoresAsync(getStoreLocationsRequest);
    //    //}

    //    ////////////////////////////////////////////////////////////////////////////////////////////////////
    //    /// <summary>   (An Action that handles HTTP POST requests) gets a store. </summary>
    //    ///
    //    /// <remarks>   Mcdand, 2/20/2018. </remarks>
    //    ///
    //    /// <param name="getStoreRequest">  The get store request. </param>
    //    ///
    //    /// <returns>   An asynchronous result that yields the store. </returns>
    //    ////////////////////////////////////////////////////////////////////////////////////////////////////

    //    //[HttpPost]
    //    //[Route("api/mobile/locator/getStore")]
    //    //public async Task<Store> GetStore([FromBody]GetStoreRequest getStoreRequest)
    //    //{
    //    //    return await service.GetStoreAsync(getStoreRequest);

    //    //}
          

    }
}
