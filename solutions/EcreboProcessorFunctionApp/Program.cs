using AutoMapper.Configuration;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SEG.EReceiptsLibrary.Implementation;
using SEG.EReceiptsLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcreboProcessorFunctionApp
{
    public class Program
    {
        public static void Main()
        {
            var host = new HostBuilder()
                 .ConfigureFunctionsWorkerDefaults(worker =>
                 {
                     worker.UseNewtonsoftJson();
                     worker.UseFunctionExecutionMiddleware();
                 })

                 .ConfigureServices(service =>
                     service.AddSingleton<IEReceiptsCosmosDAL>(InitializeCosmosClientInstanceAsync()))

                 //.ConfigureServices(service =>
                 //    service.AddTransient<IEReceiptsBlobDAL, EReceiptsBlobDAL>())

                 .ConfigureOpenApi()
                 .Build();

            host.Run();
        }

        private static EReceiptsCosmosDAL InitializeCosmosClientInstanceAsync()
        {
            string databaseName = Environment.GetEnvironmentVariable("CosmosDataBase");
            string containerName = Environment.GetEnvironmentVariable("CosmosContainer");
            string account = Environment.GetEnvironmentVariable("CosmosEndpoint");
            string key = Environment.GetEnvironmentVariable("CosmosPrimary");
            Microsoft.Azure.Cosmos.CosmosClient client = new Microsoft.Azure.Cosmos.CosmosClient(account, key);
            EReceiptsCosmosDAL cosmosDbService = new EReceiptsCosmosDAL(client, databaseName, containerName);
            // Microsoft.Azure.Cosmos.DatabaseResponse database = await client.CreateDatabaseIfNotExistsAsync(databaseName);
            //await database.Database.CreateContainerIfNotExistsAsync(containerName, "/id");

            return cosmosDbService;
        }

    }
}
