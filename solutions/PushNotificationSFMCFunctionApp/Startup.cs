using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.DependencyInjection;
using PushNotificationSFMCFunctionApp;
using PushNotificationSFMCFunctionApp.Implementation;
using PushNotificationSFMCFunctionApp.Interface;
using PushNotificationSFMCFunctionApp.Middleware;
using System;
using System.Collections.Generic;
using System.Text;


[assembly: WebJobsStartup(typeof(Startup))]
namespace PushNotificationSFMCFunctionApp
{

    public class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            //builder.AddSwashBuckle(Assembly.GetExecutingAssembly());
            Configure(new FunctionsHostBuilder(builder.Services));
        }

        private static void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddTransient<IEagleEyeMBOIssuanceEventData, EagleEyeMBOIssuanceEventDataDAL>();

        }

        internal class FunctionsHostBuilder : IFunctionsHostBuilder
        {
            public FunctionsHostBuilder(IServiceCollection services)
            {
                services.AddTransient<RequestBodyLoggingMiddleware>();
                var serviceCollection = services;
                Services = serviceCollection ?? throw new ArgumentNullException(nameof(services));
            }

            public IServiceCollection Services { get; }
        }
    }
}

