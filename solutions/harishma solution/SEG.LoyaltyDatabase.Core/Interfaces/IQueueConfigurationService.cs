using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SEG.ApiService.Models.Database;

namespace SEG.LoyaltyDatabase.Core.Interfaces
{
    public interface IQueueConfigurationService
    {
        Task<QueueConfiguration> GetConfigurationByQueueNameAsync(string queueName);

        /// <summary>   Gets active queue names. </summary>
        ///
        /// <remarks>   Mcdand, 2/19/2018. </remarks>
        ///
        /// <returns>   The active queue names. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        Task<List<string>> GetActiveQueueNamesAsync();
    }
}