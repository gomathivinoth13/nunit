using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RealTimePointsProcessFunctionApp.Implementation;
using RealTimePointsProcessFunctionApp.Interface;
using SalesForceLibrary.SalesForceAPIM;
using SEG.EagleEyeLibrary;
using SEG.EagleEyeLibrary.Controllers;
using SEG.EagleEyeLibrary.Process;
using System;

namespace RealTimePointsProcessFunctionApps
{
    public class Program
    {
        public static void Main()
        {
            var SalesForceAPIMAuthEndPoint = "https://dev-api.segrocers.com/sfmcauthservice/";
            var SalesForceAPIMBaseEndPoint = "https://dev-api.segrocers.com/sfmcservice/";
            var SEGClientID = "0yua73njn9z82zvi158d73th";
            var SEGClientSecret = "efb4ZlZdgjQbcnwfgSijz9uE";
            var RedisConnectionString = "Redis-SFMC-Cache-Dev.redis.cache.windows.net:6380,password=zdRIbodB+ukQ49AXQ1qHubSWJVsMyTzvBxN3CJK502c=,ssl=true,abortConnect=False";
            var OcpApimSubscriptionKey = "4f3d103db654429796e24164fdc97930";
            var ClientIDEE = "loexzymj7g3ncjtvl4rq";
            var BaseUrlCampaignsEE = "https://dev-api.segrocers.com/EagleEyeCampaignsAPI";
            var BaseUrlEE = "https://dev-api.segrocers.com/EE";
            var SecretEE = "wcxp8l9aof43gq39ro94uitcb6wnkh";
            var LoyaltyAzureConnection = "Server=tcp:tablestorageexport.database.windows.net,1433;Database=Loyalty;User ID=tablestorageadmin@tablestorageexport;Password=Admin123!;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30";
            var CacheConnectionString = "redis-omnichannel-seg-dev.redis.cache.windows.net:6380,password=KU76o+n4a5RlEtAMgZ85zl368ymlRDuJRdGilE77GYU=,ssl=true,abortConnect=False,connectTimeout=15000";
            var CosmosEndpointUri = "https://cosmos-omnichannel-dev.documents.azure.com:443/";
            var CosmosPrimaryKey = "TYpuvqXWw6NrvLncRURydSdLHyuAsnvLVUkbBcGjsxwrRCnSkIzbU9Hmh1LZQPO79nVRtvMkje7ZS3UbBZzjWQ==";
            var CosmosDataBaseId = "seg-stream";
            var CosmosContainerId = "coupon";
            var CacheServer = "redis-omnichannel-seg-dev.redis.cache.windows.net:6380";

            var host = new HostBuilder()
                 .ConfigureFunctionsWorkerDefaults()
                 .ConfigureServices(service =>
                     service.AddTransient<ISfmcService, SfmcService>())
                 .ConfigureServices(service =>
                      service.AddSingleton(new SalesForceAPIMService("https://dev-api.segrocers.com/sfmcauthservice/", "https://dev-api.segrocers.com/sfmcservice/",
                                                  "0yua73njn9z82zvi158d73th", "efb4ZlZdgjQbcnwfgSijz9uE", "Redis-SFMC-Cache-Dev.redis.cache.windows.net:6380,password=zdRIbodB+ukQ49AXQ1qHubSWJVsMyTzvBxN3CJK502c=,ssl=true,abortConnect=False", "4f3d103db654429796e24164fdc97930")))
                 .ConfigureServices(service =>
                     service.AddSingleton(new EagleEyeProcess("loexzymj7g3ncjtvl4rq", "wcxp8l9aof43gq39ro94uitcb6wnkh", "https://dev-api.segrocers.com/EE", "https://dev-api.segrocers.com/EagleEyeCampaignsAPI", "redis-omnichannel-seg-dev.redis.cache.windows.net:6380,password=KU76o+n4a5RlEtAMgZ85zl368ymlRDuJRdGilE77GYU=,ssl=true,abortConnect=False,connectTimeout=15000", "https://cosmos-omnichannel-dev.documents.azure.com:443/", "TYpuvqXWw6NrvLncRURydSdLHyuAsnvLVUkbBcGjsxwrRCnSkIzbU9Hmh1LZQPO79nVRtvMkje7ZS3UbBZzjWQ==", "seg-stream",
                    "coupon", "redis-omnichannel-seg-dev.redis.cache.windows.net:6380", "4f3d103db654429796e24164fdc97930", "Server=tcp:tablestorageexport.database.windows.net,1433;Database=Loyalty;User ID=tablestorageadmin@tablestorageexport;Password=Admin123!;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30")
                ))
                .ConfigureServices(service =>
                service.AddSingleton(
                    new EagleEyeService("loexzymj7g3ncjtvl4rq", "wcxp8l9aof43gq39ro94uitcb6wnkh", "https://dev-api.segrocers.com/EE", "https://dev-api.segrocers.com/EagleEyeCampaignsAPI", "4f3d103db654429796e24164fdc97930")
                ))
                 .ConfigureServices(service =>
                service.AddSingleton(
                    new EagleEyeDAL("loexzymj7g3ncjtvl4rq", "wcxp8l9aof43gq39ro94uitcb6wnkh", "https://dev-api.segrocers.com/EE", "https://dev-api.segrocers.com/EagleEyeCampaignsAPI", "4f3d103db654429796e24164fdc97930")
                ))
                 .ConfigureOpenApi()
                 .Build();

            host.Run();


        }
    }

}
