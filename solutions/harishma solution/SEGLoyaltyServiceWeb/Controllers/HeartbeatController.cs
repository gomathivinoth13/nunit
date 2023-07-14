////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	Controllers\HeartbeatController.cs
//
// summary:	Implements the heartbeat controller class
////////////////////////////////////////////////////////////////////////////////////////////////////

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SEG.ApiService.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SEG.LoyaltyServiceWeb.Controllers
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   A controller for handling heartbeats. </summary>
    ///
    /// <remarks>   Mcdand, 2/20/2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    ///
    public class HeartbeatController : Controller
    {

        SEG.CustomerLibrary.InternalCustomerController internalCustomerController;

        IConfiguration Configuration;

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="configuration">    The configuration. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public HeartbeatController(IConfiguration configuration)
        {
            Configuration = configuration;
            internalCustomerController = new CustomerLibrary.InternalCustomerController(Configuration["Settings:LoyaltyEndPoint"], Configuration["Settings:SalesForce:ocpApimSubscriptionKey"]);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   GET: api/Heartbeat. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <returns>   A string. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public async Task<Tuple<bool, string, TimeSpan>> Get(bool roundTrip = true)
        {
            if (roundTrip)
                return await CheckConnectivitiy();
            else
                return new Tuple<bool, string, TimeSpan>(true, null, new TimeSpan());
        }




        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets local IP address. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <returns>   The local IP address. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private List<string> GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            List<string> results = new List<string>();
            foreach (var ip in host.AddressList)
            {

                results.Add(ip.ToString());

            }
            return results;
        }


        private async Task<Tuple<bool, string, TimeSpan>> CheckConnectivitiy()
        {
            Stopwatch watch = Stopwatch.StartNew();
            var customerSearchResults = await new CustomerLibrary.InternalCustomerController(Configuration["Settings:LoyaltyEndPoint"], Configuration["Settings:SalesForce:ocpApimSubscriptionKey"]).CustomerSearch(new CustomerSearchRequest() { MemberId = "SEG0000000000001" }).ConfigureAwait(false);
            watch.Stop();
            return new Tuple<bool, string, TimeSpan>(customerSearchResults.IsSuccessful, customerSearchResults.ErrorMessage, watch.Elapsed);

        }
    }
}
