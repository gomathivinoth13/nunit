using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Extensions.Hosting;
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
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults(worker => {
                    worker.UseNewtonsoftJson();
                    worker.UseFunctionExecutionMiddleware();
                })
                .Build();

            await host.RunAsync();
        }
    }

}
