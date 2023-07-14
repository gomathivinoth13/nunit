using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using SEG.EReceiptsLibrary.Implementation;
using SEG.EReceiptsLibrary.Interfaces;
using SEG.EReceiptsLibrary.Models;
using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.Cosmos.Serialization.HybridRow;
using System.Net;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcreboProcessorFunctionApp.Functions
{
    public class ProcessJsonFunction
    {
        private readonly IEReceiptsCosmosDAL _eReceiptsCosmosDAL;
        public ProcessJsonFunction(IEReceiptsCosmosDAL eReceiptsCosmosDAL)
        {
            _eReceiptsCosmosDAL = eReceiptsCosmosDAL;
        }

        [Function("EventGridTriggerBlobJson")]
        public async Task Run([EventGridTrigger] EventGridEvent eventGridEvent, FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("EventGridFunction");
            try
            {
                logger.LogInformation(eventGridEvent.Data.ToString());

                StorageBlobCreatedEventData request = System.Text.Json.JsonSerializer.Deserialize<StorageBlobCreatedEventData>(eventGridEvent.Data.ToString(), new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                var blobName = GetBlobNameFromUrl(request.Url);

                if (!string.IsNullOrEmpty(blobName))
                {

                    await _eReceiptsCosmosDAL.ProcessQueueJson(blobName, logger).ConfigureAwait(false);
                }
                else
                {
                    logger.LogError("Error: {0}, Message: {1}", "blobName empty", blobName);
                }
                               
            }
            catch (Exception e)
            {
                Exception baseException = e.GetBaseException();
                logger.LogError(e, "Error: {0}, Message: {1}", e.Message, baseException.Message);

            }


        }

        private static string GetBlobNameFromUrl(string bloblUrl)
        {
            var uri = new Uri(bloblUrl);
            var blobClient = new BlobClient(uri);
            return blobClient.Name;
        }
    }
}
