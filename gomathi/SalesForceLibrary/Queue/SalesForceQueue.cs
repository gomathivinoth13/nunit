using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using SalesForceLibrary.Models;
using SEG.ApiService.Models.Queueing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace SalesForceLibrary.Queue
{
    /// <summary>
    /// 
    /// </summary>
    public class SalesForceQueue
    {

        #region Static Variables
        /// <summary>   The logging. </summary>
        private ILog Logging = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion Static Variables  
        /// <summary>
        /// 
        /// </summary>
        public string AzureStorageConnectionString { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="azureStorageConnectionString"></param>
        public SalesForceQueue(string azureStorageConnectionString)
        {
            AzureStorageConnectionString = azureStorageConnectionString;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="salesForceQueueRequest"></param>
        /// <returns></returns>
        public async Task InsertSalesForceQueue(SalesForceQueueRequest salesForceQueueRequest)
        {
            try
            {
                if (salesForceQueueRequest.customer == null) return;

                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(AzureStorageConnectionString);
                // Create the queue client
                CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
                // Retrieve a reference to a queue
                CloudQueue queue = queueClient.GetQueueReference(QueueNameType.SalesForce);
                // Create the queue if it doesn't already exist
                await queue.CreateIfNotExistsAsync().ConfigureAwait(false);

                QueueTask salesForceTask = new QueueTask
                {
                    MethodName = QueueMethodNameType.SalesForceMethodName,
                    QueueName = QueueNameType.SalesForce,
                    QueueObject = salesForceQueueRequest,
                    ContinueOnError = true,
                    ApiTransactionId = log4net.LogicalThreadContext.Properties["apitransactionid"] == null ? String.Empty : log4net.LogicalThreadContext.Properties["apitransactionid"].ToString()
                };
                ;
                Newtonsoft.Json.JsonSerializerSettings settings = new JsonSerializerSettings() { StringEscapeHandling = Newtonsoft.Json.StringEscapeHandling.EscapeNonAscii };

                //serialize the salesForceTask object and add it to the queue
                string json = JsonConvert.SerializeObject(salesForceTask, settings).ToString();
                CloudQueueMessage message = new CloudQueueMessage(json);
                await queue.AddMessageAsync(message).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Logging.Error(String.Format("An error occured while trying to run SalesForce_InsertQueue.  Error {0}", ex.Message), ex);
                throw;
            }
        }
        
    }
}
