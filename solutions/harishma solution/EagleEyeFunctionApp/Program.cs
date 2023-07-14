using EagleEyeFunctionApp.Middleware;
using Microsoft.Azure.Functions.Worker.Configuration;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SEG.EagleEyeLibrary;
using SEG.EagleEyeLibrary.Process;
using System;
using System.Threading.Tasks;


namespace EagleEyeFunctionApp
{
    public class Program
    {
        public static void Main()
        {
            var clientIDEE = Environment.GetEnvironmentVariable("ClientIDEE");
            var secretEE = Environment.GetEnvironmentVariable("SecretEE");
            var baseUrlEE = Environment.GetEnvironmentVariable("BaseUrlEE");
            var baseUrlCampaignsEE = Environment.GetEnvironmentVariable("BaseUrlCampaignsEE");
            var cacheConnectionString = Environment.GetEnvironmentVariable("CacheConnectionString");
            var cosmosEndpointUri = Environment.GetEnvironmentVariable("CosmosEndpointUri");
            var cosmosPrimaryKey = Environment.GetEnvironmentVariable("CosmosPrimaryKey");
            var cosmosDataBaseId = Environment.GetEnvironmentVariable("CosmosDataBaseId");
            var cosmosContainerId = Environment.GetEnvironmentVariable("CosmosContainerId");
            var cacheServer = Environment.GetEnvironmentVariable("CacheServer");
            var ocpApimSubscriptionKey = Environment.GetEnvironmentVariable("OcpApimSubscriptionKey");
            var loyaltyAzureConnection = Environment.GetEnvironmentVariable("LoyaltyAzureConnection");
            var couponLimitDaysString = Environment.GetEnvironmentVariable("CouponLimitDays");
            if (couponLimitDaysString == null) throw new Exception("CouponLimitDays is null");

            var couponLimitDays = Convert.ToDouble(couponLimitDaysString);
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults(worker =>
                {
                    worker.UseNewtonsoftJson();
                    //worker.UseFunctionExecutionMiddleware();
                    worker.UseMiddleware<ExceptionHandlingMiddleware>();
                })
                //.ConfigureServices(service =>
                //service.AddSingleton(
                //    new EagleEyeProcess(clientIDEE, secretEE, baseUrlEE, baseUrlCampaignsEE, cacheConnectionString, cosmosEndpointUri, cosmosPrimaryKey, cosmosDataBaseId, cosmosContainerId, cacheServer, ocpApimSubscriptionKey, loyaltyAzureConnection)
                //))
                .ConfigureServices(service =>
                     service.AddSingleton<EagleEyeProcess>(InitializeCosmosClientInstanceAsync(clientIDEE, secretEE, baseUrlEE, baseUrlCampaignsEE, cacheConnectionString, cosmosEndpointUri, cosmosPrimaryKey, cosmosDataBaseId,
                    cosmosContainerId, cacheServer, ocpApimSubscriptionKey, loyaltyAzureConnection, couponLimitDays)
                ))
                .ConfigureServices(service =>
                service.AddSingleton(
                    new EagleEyeService(clientIDEE, secretEE, baseUrlEE, baseUrlCampaignsEE, ocpApimSubscriptionKey)
                ))
                .ConfigureServices(service =>
                service.AddSingleton(
                    new EagleEyePointsProcess(clientIDEE, secretEE, baseUrlEE, baseUrlCampaignsEE, ocpApimSubscriptionKey, loyaltyAzureConnection)
                ))
            .ConfigureOpenApi()
                .Build();

            host.Run();
        }

        private static EagleEyeProcess InitializeCosmosClientInstanceAsync(string clientIDEE, string secretEE, string baseUrlEE, string baseUrlCampaignsEE, string cacheConnectionString, string cosmosEndpointUri, string cosmosPrimaryKey, string cosmosDataBaseId, string cosmosContainerId, string cacheServer, string ocpApimSubscriptionKey, string loyaltyAzureConnection, double couponLimitDays)
        {
            var CosmosEndpointUri = Environment.GetEnvironmentVariable("CosmosEndpointUri");
            var CosmosPrimaryKey = Environment.GetEnvironmentVariable("CosmosPrimaryKey");
            Microsoft.Azure.Cosmos.CosmosClient client = new Microsoft.Azure.Cosmos.CosmosClient(CosmosEndpointUri, CosmosPrimaryKey);
            EagleEyeProcess cosmosDbService = new EagleEyeProcess(client, clientIDEE, secretEE, baseUrlEE, baseUrlCampaignsEE, cacheConnectionString, cosmosEndpointUri, cosmosPrimaryKey, cosmosDataBaseId, cosmosContainerId, cacheServer, ocpApimSubscriptionKey, loyaltyAzureConnection, couponLimitDays);
            // Microsoft.Azure.Cosmos.DatabaseResponse database = await client.CreateDatabaseIfNotExistsAsync(databaseName);
            //await database.Database.CreateContainerIfNotExistsAsync(containerName, "/id");
            return cosmosDbService;
        }
    }
}
