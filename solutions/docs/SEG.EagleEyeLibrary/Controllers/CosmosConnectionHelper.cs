using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Text;

namespace SEG.EagleEyeLibrary.Controllers
{
    public class CosmosConnectionHelper
    {


        static string _cosmosEndpoint;
        static string _cosmosPrimary;
        private static CosmosClient cosmosClient;

        public static string cosmosEndpoint
        {
            get
            {

                return _cosmosEndpoint;
            }

            set
            {
                _cosmosEndpoint = value;
            }
        }
        public static string cosmosPrimary
        {
            get
            {
                return _cosmosPrimary;
            }

            set
            {
                _cosmosPrimary = value;
            }
        }


        static CosmosConnectionHelper()
        {

        }

        public static CosmosClient getCosmosClient()
        {
            cosmosClient = new CosmosClient(cosmosEndpoint, cosmosPrimary, new CosmosClientOptions()
            {
                ConnectionMode = ConnectionMode.Gateway
            });

            return cosmosClient;
        }
    }
}