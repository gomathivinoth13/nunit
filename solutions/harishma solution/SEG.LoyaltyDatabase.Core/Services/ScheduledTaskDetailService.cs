////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	DataAccess\ScheduledTaskDetailDAL.cs
//
// summary:	Implements the scheduled task detail dal class
////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using SEG.LoyaltyDatabase.Core.Interfaces;
using SEG.ApiService.Models.Database;
using SEG.LoyaltyDatabase.Models;

namespace SEG.LoyaltyDatabase.Core.Services
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   A scheduled task detail dal. </summary>
    ///
    /// <remarks>   Mcdand, 2/19/2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public class ScheduledTaskDetailService : IScheduledTaskDetailService
    {
        private readonly IUnitOfWork _unitOfWork;

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Inserts the given record. </summary>
        ///
        /// <remarks>   Mcdand, 2/19/2018. </remarks>
        ///
        /// <param name="record">   The record. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public ScheduledTaskDetailService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> InsertAsync(ScheduledTaskDetail record)
        {
            return await _unitOfWork.ScheduledTaskDetailRepository.InsertAsync(record);
        }

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
            var queryStringDelete = @"DELETE FROM ScheduledTaskDetail WHERE CreateDateTime < @DeleteDate;";
            return await _unitOfWork.ScheduledTaskDetailRepository.ExecuteSqlAsync(queryStringDelete, new { deleteDate });
        }

    }
}
