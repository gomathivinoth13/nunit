using log4net;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using SEG.ApiService.Models.Excentus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using SEG.SalesForce.Models;
using StackExchange.Redis;
/// <summary>
/// 
/// </summary>
namespace SEG.SalesForce.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class ManageAccessToken
    {
        private ILog Logging = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>   The REST dal. </summary>
        SalesForceRestDAL restDAL;

        /// <summary>
        /// 
        /// </summary>
        public string AccountID { get; set; }
      
        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseRestUrl"></param>
        /// <param name="clientID"></param>
        /// <param name="clientSecret"></param>
        public ManageAccessToken(string baseRestUrl, string clientID, string clientSecret)
        {
            restDAL = new SalesForceRestDAL(baseRestUrl, clientID, clientSecret);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<string> PosGetAccessToken()
        {
            string accessToken = null;
            AccessTokenResponse apiAccessToken = new AccessTokenResponse();

            try
            {

                apiAccessToken = await restDAL.PosAPIAccesstokenGenerate().ConfigureAwait(false);

                if (apiAccessToken != null)
                {
                    accessToken = apiAccessToken.accessToken;
                }
                else
                {
                    Exception ex = new Exception("Invalid accessToken , null access token");
                    throw ex;
                }


                return accessToken;

            }
            catch (Exception ex)
            {
                Logging.Error(String.Format("An error occured while trying to run SalesForce_ManageAccessToken.  Error {0}", ex.Message), ex);
                throw;
            }
        }
    }
}
