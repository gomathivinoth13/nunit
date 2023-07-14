using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SEG.ApiService.Models.Database;
using SEG.ApiService.Models.Queueing;

namespace SEG.LoyaltyDatabase.Core.Interfaces
{
    public interface ILoyaltyDatabaseService
    {
        Task<List<StagedMembershipRecord>> GetStagedMembershipRecordsByMemberIdAsync(string memberId);

        /// <summary>   Saves a staged membership record. </summary>
        ///
        /// <remarks>   Mcdand, 2/19/2018. </remarks>
        ///
        /// <param name="record">   The record. </param>
        ///
        /// <returns>   True if it succeeds, false if it fails. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        Task<bool> SaveStagedMembershipRecord(StagedMembershipRecord record);

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
        Task<bool> DeleteStagedMembershipRecord(string stagedMembershipRecordId);

        /// <summary>   Gets configuration by queue name. </summary>
        ///
        /// <remarks>   Mcdand, 2/19/2018. </remarks>
        ///
        /// <param name="queueName">    Name of the queue. </param>
        ///
        /// <returns>   The configuration by queue name. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        Task<QueueConfiguration> GetConfigurationByQueueNameAsync(string queueName);

        /// <summary>   Gets active queue names. </summary>
        ///
        /// <remarks>   Mcdand, 2/19/2018. </remarks>
        ///
        /// <returns>   The active queue names. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        Task<List<string>> GetActiveQueueNamesAsync();

        /// <summary>   Adds an error task to dead queue. </summary>
        ///
        /// <remarks>   Mcdand, 2/19/2018. </remarks>
        ///
        /// <param name="errorQueueTask">   The error queue task. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        Task<bool> AddErrorTaskToDeadQueueAsync(ErrorQueueTask errorQueueTask);

        /// <summary>   Deletes the internal log records by date described by deleteDate. </summary>
        ///
        /// <remarks>   Mcdand, 2/19/2018. </remarks>
        ///
        /// <param name="deleteDate">   The delete date. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        Task<bool> DeleteInternalLogRecordsByDateAsync(DateTime deleteDate);

        /// <summary>
        /// Deletes the scheduled task detail records by date described by deleteDate.
        /// </summary>
        ///
        /// <remarks>   Mcdand, 2/19/2018. </remarks>
        ///
        /// <param name="deleteDate">   The delete date. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        Task<bool> DeleteScheduledTaskDetailRecordsByDateAsync(DateTime deleteDate);
    }
}