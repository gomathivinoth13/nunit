using System;
using System.Threading.Tasks;
using SEG.ApiService.Models.Database;
using SEG.LoyaltyDatabase.Models;

namespace SEG.LoyaltyDatabase.Core.Interfaces
{
    public interface IScheduledTaskDetailService
    {
        Task<bool> InsertAsync(ScheduledTaskDetail record);

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