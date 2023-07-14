////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	LoyaltyDatabaseManager.cs
//
// summary:	Implements the loyalty database manager class
////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using log4net;
using SEG.ApiService.Models.Database;
using SEG.ApiService.Models.Queueing;
using SEG.LoyaltyDatabase.Core.Interfaces;

namespace SEG.LoyaltyDatabase.Core.Services
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   Manager for loyalty databases. </summary>
    ///
    /// <remarks>   Mcdand, 2/19/2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public class LoyaltyDatabaseService : ILoyaltyDatabaseService
    {
        private readonly IStagedMembershipRecordService _stagedMembershipRecordService;
        private readonly IQueueConfigurationService _queueConfigurationService;
        private readonly IErrorQueueService _errorQueueService;
        private readonly ILogService _logService;
        private readonly IScheduledTaskDetailService _scheduledTaskDetailService;
        #region Static Variables

        private ILog Logging = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion Static Variables    

        #region Public Methods

        #region Staged Membership Record

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets staged membership records by member identifier. </summary>
        /// 
        /// <remarks>   Mcdand, 2/19/2018. </remarks>
        /// 
        /// <param name="memberId"> Identifier for the member. </param>
        /// <param name="stagedMembershipRecordService"></param>
        /// <param name="queueConfigurationService"></param>
        /// <param name="deadQueueService"></param>
        /// <param name="logService"></param>
        /// <param name="scheduledTaskDetailService"></param>
        /// <returns>   The staged membership records by member identifier. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public LoyaltyDatabaseService(IStagedMembershipRecordService stagedMembershipRecordService, IQueueConfigurationService queueConfigurationService,
            IErrorQueueService deadQueueService, ILogService logService, IScheduledTaskDetailService scheduledTaskDetailService)
        {
            _stagedMembershipRecordService = stagedMembershipRecordService;
            _queueConfigurationService = queueConfigurationService;
            _errorQueueService = deadQueueService;
            _logService = logService;
            _scheduledTaskDetailService = scheduledTaskDetailService;
        }

        public async Task<List<StagedMembershipRecord>> GetStagedMembershipRecordsByMemberIdAsync(string memberId)
        {
            try
            {
                return await _stagedMembershipRecordService.GetStagedMembershipRecordsByMemberIdAsync(memberId);
            }
            catch (Exception ex)
            {
                string error = string.Format(@"An Exception occurred while trying to GetStagedMembershipRecordsByMemberId,
                    Exception: {0}", ex.Message);
                Logging.Error(error, ex);

                throw;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Saves a staged membership record. </summary>
        ///
        /// <remarks>   Mcdand, 2/19/2018. </remarks>
        ///
        /// <param name="record">   The record. </param>
        ///
        /// <returns>   True if it succeeds, false if it fails. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public async Task<bool> SaveStagedMembershipRecord(StagedMembershipRecord record)
        {
            try
            {
                return await _stagedMembershipRecordService.InsertAsync(record);
            }
            catch (Exception ex)
            {
                string error = string.Format(@"An Exception occurred while trying to Insert a StagedMembershipRecord,
                    Exception: {0}", ex.Message);
                Logging.Error(error, ex);

                throw;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Deletes the staged membership record described by stagedMembershipRecordId.
        /// </summary>
        ///
        /// <remarks>   Mcdand, 2/19/2018. </remarks>
        ///
        /// <param name="stagedMembershipRecordId"> Identifier for the staged membership record. </param>
        ///
        /// <returns>   True if it succeeds, false if it fails. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public async Task<bool> DeleteStagedMembershipRecord(string stagedMembershipRecordId)
        {
            try
            {
                return await _stagedMembershipRecordService.DeleteAsync(stagedMembershipRecordId);
            }
            catch (Exception ex)
            {
                string error = string.Format(@"An Exception occurred while trying to Delete a StagedMembershipRecord,
                    Exception: {0}", ex.Message);
                Logging.Error(error, ex);

                throw;
            }
        }

        #endregion Staged Membership Record

        #region Queue Configuration

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets configuration by queue name. </summary>
        ///
        /// <remarks>   Mcdand, 2/19/2018. </remarks>
        ///
        /// <param name="queueName">    Name of the queue. </param>
        ///
        /// <returns>   The configuration by queue name. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public async Task<QueueConfiguration> GetConfigurationByQueueNameAsync(string queueName)
        {
            try
            {
                return await _queueConfigurationService.GetConfigurationByQueueNameAsync(queueName);
            }
            catch (Exception ex)
            {
                string error = string.Format(@"An Exception occurred while trying to retreive the queue configuration for the {0} queue. Method: {1},
                    Exception: {2}, Trace{3}", queueName, "GetConfigurationByQueueName", ex.Message, ex.StackTrace);
                Logging.Error(error, ex);

                throw;
            }
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
            try
            {
                return await _queueConfigurationService.GetActiveQueueNamesAsync();
            }
            catch (Exception ex)
            {
                string error = string.Format(@"An Exception occurred while trying to retreive the active queues. Method: {0},
                    Exception: {1}, Trace{2}", "GetActiveQueueNames", ex.Message, ex.StackTrace);
                Logging.Error(error, ex);

                throw;
            }
        }

        #endregion Queue Configuration

        #region Dead Queue

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Adds an error task to dead queue. </summary>
        ///
        /// <remarks>   Mcdand, 2/19/2018. </remarks>
        ///
        /// <param name="errorQueueTask">   The error queue task. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public async Task<bool> AddErrorTaskToDeadQueueAsync(ErrorQueueTask errorQueueTask)
        {
            try
            {
                return await _errorQueueService.AddErrorTaskToDeadQueueAsync(errorQueueTask);
            }
            catch (Exception ex)
            {
                string error = string.Format("An Exception occurred while trying to save an error task to the database. Method: {0},  Exception: {1}, Trace{2}",
                    "AddErrorTaskToDeadQueue", ex.Message, ex.StackTrace);
                Logging.Error(error, ex);

                throw;
            }
        }

        #endregion Dead Queue

        #region Log

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Deletes the internal log records by date described by deleteDate. </summary>
        ///
        /// <remarks>   Mcdand, 2/19/2018. </remarks>
        ///
        /// <param name="deleteDate">   The delete date. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public async Task<bool> DeleteInternalLogRecordsByDateAsync(DateTime deleteDate)
        {
            try
            {
                return await _logService.DeleteInternalLogRecordsByDateAsync(deleteDate);
            }
            catch (Exception ex)
            {
                string error = string.Format("An Exception occurred while trying to delete log records from the database. Method: {0},  Exception: {1}, Trace{2}",
                    "DeleteInternalLogRecordsByDate", ex.Message, ex.StackTrace);
                Logging.Error(error, ex);

                throw;
            }
        }

        #endregion Log

        #region ScheduledTaskDetail

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Deletes the scheduled task detail records by date described by deleteDate.
        /// </summary>
        ///
        /// <remarks>   Mcdand, 2/19/2018. </remarks>
        ///
        /// <param name="deleteDate">   The delete date. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public async Task<bool> DeleteScheduledTaskDetailRecordsByDateAsync(DateTime deleteDate)
        {
            try
            {
                return await _scheduledTaskDetailService.DeleteScheduledTaskDetailRecordsByDateAsync(deleteDate);
            }
            catch (Exception ex)
            {
                string error = string.Format("An Exception occurred while trying to delete scheduled task detail records from the database. Method: {0},  Exception: {1}, Trace{2}",
                    "DeleteLogsByDate", ex.Message, ex.StackTrace);
                Logging.Error(error, ex);

                throw;
            }
        }

        #endregion 

        #endregion Public Methods
    }
}