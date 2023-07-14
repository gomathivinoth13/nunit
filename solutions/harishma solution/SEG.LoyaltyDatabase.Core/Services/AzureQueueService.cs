////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	Utility\AzureQueue.cs
//
// summary:	Implements the azure queue class
////////////////////////////////////////////////////////////////////////////////////////////////////

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using SEG.ApiService.Models.AppSettings;
using SEG.ApiService.Models.Queueing;
using SEG.LoyaltyDatabase.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEG.ApiService.Models.Utility
{
    /////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   An utilities. </summary>
    ///
    /// <remarks>   Mcdand, 2/19/2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public class AzureQueueService : IAzureQueueService
    {

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the azure storage connection string. </summary>
        ///
        /// <value> The azure storage connection string. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public string AzureStorageConnectionString { get; set; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Mcdand, 2/19/2018. </remarks>
        ///
        /// <param name="connectionString"> The connection string. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public AzureQueueService(IOptions<AppSettingsOptions> settings)
        {
            AzureStorageConnectionString = settings.Value.AzureStorageConnectionString;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets the endif. </summary>
        ///
        /// <value> The endif. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public async Task<bool> AddQueueTaskAsync(QueueTask queueTask)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                StringEscapeHandling = StringEscapeHandling.EscapeNonAscii,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            string json = JsonConvert.SerializeObject(queueTask, settings);

            log4net.LogicalThreadContext.Properties["request"] = json;

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(AzureStorageConnectionString);
            // Create the queue client
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            // Retrieve a reference to a queue
            CloudQueue queue = queueClient.GetQueueReference(queueTask.QueueName);
            // Create the queue if it doesn't already exist


            CloudQueueMessage message = new CloudQueueMessage(json);
            await queue.CreateIfNotExistsAsync().ConfigureAwait(false);
            await queue.AddMessageAsync(message).ConfigureAwait(false);

            return true;
        }
    }
}
