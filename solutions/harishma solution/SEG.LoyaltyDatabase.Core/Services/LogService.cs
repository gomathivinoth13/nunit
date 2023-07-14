////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	DataAccess\LogDAL.cs
//
// summary:	Implements the log dal class
////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SEG.ApiService.Models.Database;
using SEG.LoyaltyDatabase.Core.Interfaces;

namespace SEG.LoyaltyDatabase.Core.Services
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   A log dal. </summary>
    ///
    /// <remarks>   Mcdand, 2/19/2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public class LogService : ILogService
    {
        private readonly IUnitOfWork _unitOfWork;

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Deletes the internal log records by date described by deleteDate. </summary>
        /// 
        /// <remarks>   Mcdand, 2/19/2018. </remarks>
        /// 
        /// <param name="deleteDate">   The delete date. </param>
        /// <param name="configuration"></param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public LogService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> DeleteInternalLogRecordsByDateAsync(DateTime deleteDate)
        {
            if (deleteDate == null) return false;

            var queryStringDelete = @"DELETE FROM dbo.Log WHERE CreateDateTime < @DeleteDate";
            await _unitOfWork.LogRepository.ExecuteSqlAsync(queryStringDelete, new { DeleteDate = deleteDate });
            return true;
        }
    }
}
