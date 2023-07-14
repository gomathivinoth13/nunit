
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using SEG.SalesForce.Models;
using StackExchange.Redis;
using SalesForceLibrary.Models;

/// <summary>
/// 
/// </summary>
namespace SEG.SalesForce.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class ManageAccessTokenV2
    {

        SalesForceRestAPIMDAL restDAL;
        RedisDatabaseOperations redisDatabaseOperations;

        private string clientIDLocal { get; set; }
        private string clientSecretLocal { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseRestUrl"></param>
        /// <param name="clientID"></param>
        /// <param name="clientSecret"></param>
        /// <param name="cacheConnectionString"></param>
        /// <param name="ocpApimSubscriptionKey"></param>
        public ManageAccessTokenV2(string baseRestUrl, string clientID, string clientSecret, string cacheConnectionString, string ocpApimSubscriptionKey)
        {
            clientIDLocal = clientID;
            clientSecretLocal = clientSecret;
            RedisConnectorHelper.localHost = cacheConnectionString;
            redisDatabaseOperations = new RedisDatabaseOperations();
            restDAL = new SalesForceRestAPIMDAL(baseRestUrl, clientID, clientSecret, ocpApimSubscriptionKey);
        }

        /// <summary>
        /// get access token 
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetAccessToken()
        {
            string accessToken = null;
            AccessTokenV2 apiAccessToken = new AccessTokenV2();

            try
            {

                //Get AccessToken from Cache
                string cacheKey = string.Format("AcessToken:{0}:{1}", clientIDLocal, clientSecretLocal);

                var token = redisDatabaseOperations.getCache(cacheKey);

                if (!token.IsNullOrEmpty)
                {
                    apiAccessToken = JsonConvert.DeserializeObject<AccessTokenV2>(token);
                }
                else
                {
                    apiAccessToken = await restDAL.APIAccesstokenGenerate().ConfigureAwait(false);

                    redisDatabaseOperations.setCache(JsonConvert.SerializeObject(apiAccessToken), cacheKey, apiAccessToken.ExpiresIn - 30);

                }

                if (apiAccessToken != null)
                {
                    accessToken = apiAccessToken.AccessToken;
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
                throw;
            }
        }
    }
}
