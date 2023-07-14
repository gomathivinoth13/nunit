using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RealTimePointsProcessFunctionApp.Implementation;
using RealTimePointsProcessFunctionApp.Interface;
using SalesForceLibrary.SalesForceAPIM;
using SEG.EagleEyeLibrary.Controllers;
using SEG.EagleEyeLibrary.Process;
using SEG.EagleEyeLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EEtoSFMCIntFuncApp
{
    public class Program
    {
        static async Task Main()
        {
            var SalesForceAPIMAuthEndPoint = Environment.GetEnvironmentVariable("SalesForceAPIMAuthEndPoint");
            var SalesForceAPIMBaseEndPoint = Environment.GetEnvironmentVariable("SalesForceAPIMBaseEndPoint");
            var SEGClientID = Environment.GetEnvironmentVariable("SEG_ClientID");
            var SEGClientSecret = Environment.GetEnvironmentVariable("SEG_ClientSecret");
            var RedisConnectionString = Environment.GetEnvironmentVariable("redisConnectionString");
            var OcpApimSubscriptionKey = Environment.GetEnvironmentVariable("OcpApimSubscriptionKey");
            var ClientIDEE = Environment.GetEnvironmentVariable("ClientIDEE");
            var BaseUrlCampaignsEE = Environment.GetEnvironmentVariable("BaseUrlCampaignsEE");
            var BaseUrlEE = Environment.GetEnvironmentVariable("BaseUrlEE");
            var SecretEE = Environment.GetEnvironmentVariable("SecretEE");
            var LoyaltyAzureConnection = Environment.GetEnvironmentVariable("loyaltyAzureConnection");
            var CacheConnectionString = Environment.GetEnvironmentVariable("CacheConnectionString");
            var CosmosEndpointUri = Environment.GetEnvironmentVariable("CosmosEndpointUri");
            var CosmosPrimaryKey = Environment.GetEnvironmentVariable("CosmosPrimaryKey");
            var CosmosDataBaseId = Environment.GetEnvironmentVariable("CosmosDataBaseId");
            var CosmosContainerId = Environment.GetEnvironmentVariable("CosmosContainerId");
            var CacheServer = Environment.GetEnvironmentVariable("CacheServer");




            var host = new HostBuilder()
                 .ConfigureFunctionsWorkerDefaults()
                 .ConfigureServices(service =>
                     service.AddTransient<ISfmcService, SfmcService>())
                 .ConfigureServices(service =>
                      service.AddSingleton(new SalesForceAPIMService(SalesForceAPIMAuthEndPoint, SalesForceAPIMBaseEndPoint,
                                                  SEGClientID, SEGClientSecret, RedisConnectionString, OcpApimSubscriptionKey)))
                 .ConfigureServices(service =>
                     service.AddSingleton(new EagleEyeProcess(ClientIDEE, SecretEE, BaseUrlEE, BaseUrlCampaignsEE, CacheConnectionString, CosmosEndpointUri, CosmosPrimaryKey, CosmosDataBaseId,
                    CosmosContainerId, CacheServer, OcpApimSubscriptionKey, LoyaltyAzureConnection)
                ))
                .ConfigureServices(service =>
                service.AddSingleton(
                    new EagleEyeService(ClientIDEE, SecretEE, BaseUrlEE, BaseUrlCampaignsEE, OcpApimSubscriptionKey)
                ))
                 .ConfigureServices(service =>
                service.AddSingleton(
                    new EagleEyeDAL(ClientIDEE, SecretEE, BaseUrlEE, BaseUrlCampaignsEE, OcpApimSubscriptionKey)
                ))
                 .ConfigureOpenApi()
                 .Build();

            host.Run();






        }
    }

}
