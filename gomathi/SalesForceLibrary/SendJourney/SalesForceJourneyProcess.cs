using SalesForceLibrary.Models;
using SEG.ApiService.Models.SalesForce;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SalesForceLibrary.SendJourney
{
    /// <summary>
    /// SalesForceJourneyProcess
    /// </summary>
    public class SalesForceJourneyProcess
    {
        /// <summary>
        /// 
        /// </summary>
        public string WebApiEndPoint { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string OcpApimSubscriptionKeySecret { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="webApiEndPoint"></param>
        /// <param name="ocpApimSubscriptionKey"></param>
        public SalesForceJourneyProcess(string webApiEndPoint, string ocpApimSubscriptionKey)
        {
            WebApiEndPoint = webApiEndPoint;
            OcpApimSubscriptionKeySecret = ocpApimSubscriptionKey;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="salesForceQueueRequest"></param>
        /// <returns></returns>
        public async Task ProcessSalesForceRequest(SalesForceQueueRequest salesForceQueueRequest)
        {
            Dictionary<string, object> queryParams = new Dictionary<string, object>();

            try 
            {
                WelcomeJourneyResponse welcomeJourneyResponse = null;

                welcomeJourneyResponse = (await SEG.Shared.ApiUtility.RestfulPostAsync<WelcomeJourneyResponse>(salesForceQueueRequest, "SalesForcePosMethodName", WebApiEndPoint, QueryParams: queryParams, createHeader(OcpApimSubscriptionKeySecret)).ConfigureAwait(false)).Result;              
            }
            catch(Exception ex)
            {
                throw ex;
            }
           
        }

        private Dictionary<string, object> createHeader(string OcpApimSubscriptionKeySecret)
        {
            Dictionary<string, object> headers = new Dictionary<string, object>();

            headers.Add("Ocp-Apim-Subscription-Key", OcpApimSubscriptionKeySecret);
            return headers;
        }

    }
}
