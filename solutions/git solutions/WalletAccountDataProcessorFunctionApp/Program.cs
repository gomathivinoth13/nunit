
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WalletAccountDataProcessorFunctionApp.Implementation;
using WalletAccountDataProcessorFunctionApp.Interface;

namespace EcreboProcessorFunctionApp
{
    public class Program
    {
        public static void Main()
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults(worker => worker.UseNewtonsoftJson())
                .ConfigureServices(service =>
                    service.AddSingleton<ISetResponse,SetResponse>())

                .ConfigureServices(service =>
                    service.AddSingleton<ISfmchelper, Sfmchelper>())

                 .ConfigureServices(service => 
                    service.AddSingleton<IWalletAccountIDEventDataDAL , WalletAccountIDEventDataDAL>())

                  .ConfigureServices(service =>
                    service.AddSingleton<IProcessAccountIdData, ProcessAccountIdData>())

                .ConfigureOpenApi()
                .Build();

            host.Run();
        }

       

    }
}
