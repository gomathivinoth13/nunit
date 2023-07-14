using SalesForceLibrary.SalesForceAPIM;
using SEG.EagleEyeLibrary;
using SEG.SalesForce;
using System;
using System.Collections.Generic;
using System.Text;

namespace PushNotificationSFMCFunctionApp.Implementation
{
    public class ConfigurationDAL
    {
        private EagleEyeService serviceEE;
        string redisConnectionString { get; set; }


        //protected string _connectionString = "Server=tcp:tablestorageexport.database.windows.net,1433;Database=Loyalty;User ID=tablestorageadmin@tablestorageexport;Password=Admin123!;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        protected string _connectionString = Environment.GetEnvironmentVariable("DataBaseConnectionString");

        public ConfigurationDAL()
        {
            //redisConnectionString = "Redis-SFMC-Cache-Dev.redis.cache.windows.net:6380,password=zdRIbodB+ukQ49AXQ1qHubSWJVsMyTzvBxN3CJK502c=,ssl=True,abortConnect=False";
            redisConnectionString = Environment.GetEnvironmentVariable("RedisConnectionString");
        }

        public string setKey(string merchant_Parent_ID)
        {

            //return "APIEvent-9cce8344-82c4-4fce-bc3c-c5694e09e376";
            switch (merchant_Parent_ID.Substring(0, 1).ToLower().Trim())
            {
                case "h":
                    return Environment.GetEnvironmentVariable("HarveysEventDefinitionKey");
                case "w":
                    return Environment.GetEnvironmentVariable("WDEventDefinitionKey");
                case "f":
                    return Environment.GetEnvironmentVariable("FrescoEventDefinitionKey");
                default:
                    return Environment.GetEnvironmentVariable("WDEventDefinitionKey");

            }
        }


        public SalesForceAPIMService setServiceWelcomeJourney(string merchant_Parent_ID)
        {
            //return new SalesForceAPIMService("https://dev-api.segrocers.com/auth_sfmc/", "https://dev-api.segrocers.com/rest_sfmc/", "0yua73njn9z82zvi158d73th", "efb4ZlZdgjQbcnwfgSijz9uE", redisConnectionString, "c2bdb0112e534ba090f48a179f311f7f");


            switch (merchant_Parent_ID.Substring(0, 1).ToLower().Trim())
            {
                case "h":
                    return new SalesForceAPIMService(Environment.GetEnvironmentVariable("SalesForceAuthEndPoint"), Environment.GetEnvironmentVariable("SalesForceRestEndPoint"), Environment.GetEnvironmentVariable("Harveys_ClientID"), Environment.GetEnvironmentVariable("Harveys_ClientSecret"), redisConnectionString, Environment.GetEnvironmentVariable("ocpApimSubscriptionKey"));
                case "w":
                    return new SalesForceAPIMService(Environment.GetEnvironmentVariable("SalesForceAuthEndPoint"), Environment.GetEnvironmentVariable("SalesForceRestEndPoint"), Environment.GetEnvironmentVariable("WinnDixie_ClientID"), Environment.GetEnvironmentVariable("WinnDixie_ClientSecret"), redisConnectionString, Environment.GetEnvironmentVariable("ocpApimSubscriptionKey"));
                case "f":
                    return new SalesForceAPIMService(Environment.GetEnvironmentVariable("SalesForceAuthEndPoint"), Environment.GetEnvironmentVariable("SalesForceRestEndPoint"), Environment.GetEnvironmentVariable("Fresco_ClientID"), Environment.GetEnvironmentVariable("Fresco_ClientSecret"), redisConnectionString, Environment.GetEnvironmentVariable("ocpApimSubscriptionKey"));
                default:
                    return new SalesForceAPIMService(Environment.GetEnvironmentVariable("SalesForceAuthEndPoint"), Environment.GetEnvironmentVariable("SalesForceRestEndPoint"), Environment.GetEnvironmentVariable("WinnDixie_ClientID"), Environment.GetEnvironmentVariable("WinnDixie_ClientSecret"), redisConnectionString, Environment.GetEnvironmentVariable("ocpApimSubscriptionKey"));
            }
        }
        public EagleEyeService setService()
        {

            //serviceEE = new EagleEyeService("loexzymj7g3ncjtvl4rq", "wcxp8l9aof43gq39ro94uitcb6wnkh", "https://dev-api.segrocers.com/EE", "https://dev-api.segrocers.com/EE", "c2bdb0112e534ba090f48a179f311f7f");

            serviceEE = new EagleEyeService(Environment.GetEnvironmentVariable("clientIDEE"), Environment.GetEnvironmentVariable("secretEE"), Environment.GetEnvironmentVariable("baseUrlEE"), Environment.GetEnvironmentVariable("baseUrlCampaignsEE"), Environment.GetEnvironmentVariable("ocpApimSubscriptionKey"));

            return serviceEE;
        }
    }
}
