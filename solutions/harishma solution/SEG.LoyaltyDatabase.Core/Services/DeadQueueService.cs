////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	DataAccess\DeadQueueDAL.cs
//
// summary:	Implements the dead queue dal class
////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SEG.ApiService.Models.Queueing;
using SEG.LoyaltyDatabase.Core.Interfaces;
using SEG.ApiService.Models.Database;
using SEG.LoyaltyDatabase.Models;

namespace SEG.LoyaltyDatabase.Core.Services
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   A dead queue dal. </summary>
    ///
    /// <remarks>   Mcdand, 2/19/2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public class DeadQueueService : IErrorQueueService
    {
        private readonly IUnitOfWork _unitOfWork;

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Adds an error task to dead queue. </summary>
        /// 
        /// <remarks>   Mcdand, 2/19/2018. </remarks>
        /// 
        /// <param name="errorQueueTask">   The error queue task. </param>
        /// <param name="configuration"></param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public DeadQueueService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> AddErrorTaskToDeadQueueAsync(ErrorQueueTask errorQueueTask)
        {
            int QueueNameMaxSize = 255;
            int ErrorMaxSize = 500;
            if (errorQueueTask == null) return false;

            DeadTaskQueue dtQueue = new DeadTaskQueue
            {
                QueueName = errorQueueTask.QueueTask.QueueName.Length > QueueNameMaxSize ? errorQueueTask.QueueTask.QueueName.Substring(0, QueueNameMaxSize) : errorQueueTask.QueueTask.QueueName,
                QueueTask = JsonConvert.SerializeObject(errorQueueTask.QueueTask),
                Error = errorQueueTask.Error.Length > ErrorMaxSize ? errorQueueTask.Error.Substring(0, ErrorMaxSize) : errorQueueTask.Error,
                Exception = errorQueueTask.Exception
            };
                
            return await _unitOfWork.DeadTaskQueueRepository.InsertAsync(dtQueue);
        }
    }
}
