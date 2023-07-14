
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SalesForceLibrary.SalesForceAPIM;
using System;
using System.Threading.Tasks;
using WalletAccountDataProcessorFunctionApp.Implementation;
using WalletAccountDataProcessorFunctionApp.Interface;

namespace EcreboProcessorFunctionApp
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

            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureServices(service =>
                    service.AddTransient<ISfmcRepo, SfmcRepo>())

                .ConfigureServices(service =>
                    service.AddTransient<IWalletAccountDataProcessEventDataRepo, WalletAccountDataProcessEventDataRepo>())

                  .ConfigureServices(service =>
                    service.AddTransient<IAccountLogRepository, AccountLogRepository>())

                .ConfigureServices(service =>
                     service.AddSingleton(new SalesForceAPIMService(SalesForceAPIMAuthEndPoint, SalesForceAPIMBaseEndPoint,
                                                 SEGClientID, SEGClientSecret, RedisConnectionString, OcpApimSubscriptionKey)))
                .ConfigureOpenApi()
                .Build();

            await host.RunAsync();
        }
    }
}
