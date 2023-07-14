using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using SEG.EReceiptsLibrary.Implementation;
using SEG.EReceiptsLibrary.Interfaces;
using SEG.EReceiptsLibrary.Models;
using Azure.Storage.Blobs;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.EventGrid.Models;
using System.Net.Http;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace EcreboProcessorFunctionApp.Functions
{
    public static class ProcessHtmlFunction
    {
        private static IEReceiptsBlobDAL _eReceiptsBlobDAL;


        [Function("EventGridTriggerBlobHtml")]
        public static async Task<IActionResult> Run([EventGridTrigger] EventGridEvent eventGridEvent, FunctionContext executionContext)
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
                    _eReceiptsBlobDAL = new EReceiptsBlobDAL();
                    await _eReceiptsBlobDAL.ProcessQueueHtml(blobName, logger).ConfigureAwait(false);
                }
                else
                {
                    logger.LogError("Error: {0}, Message: {1}", "blobName empty", blobName);
                }

                return new OkResult();
            }
            catch (Exception e)
            {
                Exception baseException = e.GetBaseException();
                logger.LogError(e, "Error: {0}, Message: {1}", e.Message, baseException.Message);

                return new BadRequestResult();
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

