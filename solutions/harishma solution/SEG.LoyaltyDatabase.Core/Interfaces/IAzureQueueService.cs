////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	Utility\AzureQueue.cs
//
// summary:	Implements the azure queue class
////////////////////////////////////////////////////////////////////////////////////////////////////

using SEG.ApiService.Models.Queueing;
using System.Threading.Tasks;

namespace SEG.LoyaltyDatabase.Core.Interfaces
{
    public interface IAzureQueueService
    {
        string AzureStorageConnectionString { get; set; }

        Task<bool> AddQueueTaskAsync(QueueTask queueTask);
    }
}