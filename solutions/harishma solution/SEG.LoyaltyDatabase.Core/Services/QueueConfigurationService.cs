////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	DataAccess\QueueConfigurationDAL.cs
//
// summary:	Implements the queue configuration dal class
////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using SEG.ApiService.Models.Database;
using SEG.LoyaltyDatabase.Core.Interfaces;

namespace SEG.LoyaltyDatabase.Core.Services
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   A queue configuration dal. </summary>
    ///
    /// <remarks>   Mcdand, 2/19/2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public class QueueConfigurationService : IQueueConfigurationService
    {
        private readonly IUnitOfWork _unitOfWork;

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets configuration by queue name. </summary>
        ///
        /// <remarks>   Mcdand, 2/19/2018. </remarks>
        ///
        /// <param name="queueName">    Name of the queue. </param>
        ///
        /// <returns>   The configuration by queue name. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public QueueConfigurationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<QueueConfiguration> GetConfigurationByQueueNameAsync(string queueName)
        {
            var result = await _unitOfWork.QueueConfigurationRepository.GetAsync(q => q.QueueName == queueName);
            return result.FirstOrDefault();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets active queue names. </summary>
        ///
        /// <remarks>   Mcdand, 2/19/2018. </remarks>
        ///
        /// <returns>   The active queue names. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public async Task<List<string>> GetActiveQueueNamesAsync()
        {
            var results = await _unitOfWork.QueueConfigurationRepository.GetAsync(q => q.IsActive == true);
            return results.Select(s => s.QueueName).ToList();
        }
    }
}
