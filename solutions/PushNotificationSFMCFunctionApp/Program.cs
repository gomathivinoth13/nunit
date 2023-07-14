using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PushNotificationSFMCFunctionApp.Implementation;
using PushNotificationSFMCFunctionApp.Interface;
using PushNotificationSFMCFunctionApp.Middleware;
using System;

namespace MessagePoller
{
    public class Program
    {

        public static void Main()
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureServices(s =>
                {
                    s.AddTransient<IEagleEyeMBOIssuanceEventData, EagleEyeMBOIssuanceEventDataDAL>();
                    s.AddTransient<RequestBodyLoggingMiddleware>();
                    var serviceCollection = s;
                    IServiceCollection Services = serviceCollection ?? throw new ArgumentNullException(nameof(s));

                })
                .Build();

            host.Run();
        }
        //public void Configure(IWebJobsBuilder builder)
        //{
        //    //builder.AddSwashBuckle(Assembly.GetExecutingAssembly());
        //   // Configure(new FunctionsHostBuilder(builder.Services));
        //}
        //internal class FunctionsHostBuilder : IFunctionsHostBuilder
        //{
        //    public FunctionsHostBuilder(IServiceCollection services)
        //    {
        //        services.AddTransient<RequestBodyLoggingMiddleware>();
        //        var serviceCollection = services;
        //        Services = serviceCollection ?? throw new ArgumentNullException(nameof(services));
        //    }


        //}
    }
}
