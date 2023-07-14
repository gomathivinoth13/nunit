////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	DataAccess\StagedMembershipRecordDAL.cs
//
// summary:	Implements the staged membership record dal class
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
    /// <summary>   A staged membership record dal. </summary>
    ///
    /// <remarks>   Mcdand, 2/19/2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public class StagedMembershipRecordService : IStagedMembershipRecordService
    {
        private readonly IUnitOfWork _unitOfWork;


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets staged membership records by member identifier. </summary>
        /// 
        /// <remarks>   Mcdand, 2/19/2018. </remarks>
        /// 
        /// <param name="memberId"> Identifier for the member. </param>
        /// <param name="configuration"></param>
        /// <returns>   The staged membership records by member identifier. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public StagedMembershipRecordService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<StagedMembershipRecord>> GetStagedMembershipRecordsByMemberIdAsync(string memberId)

        {
            var results = await _unitOfWork.StagedMembershipRecordRepository.GetAsync(s => s.MemberId == memberId);
            return results.ToList();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Inserts the given record. </summary>
        ///
        /// <remarks>   Mcdand, 2/19/2018. </remarks>
        ///
        /// <param name="record">   The record. </param>
        ///
        /// <returns>   True if it succeeds, false if it fails. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public async Task<bool> InsertAsync(StagedMembershipRecord record)
        {
            return await _unitOfWork.StagedMembershipRecordRepository.InsertAsync(record, includePrimaryKey: false);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Deletes the given stagedMembershipRecordId. </summary>
        ///
        /// <remarks>   Mcdand, 2/19/2018. </remarks>
        ///
        /// <param name="stagedMembershipRecordId"> The staged membership record Identifier to delete. </param>
        ///
        /// <returns>   True if it succeeds, false if it fails. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public async Task<bool> DeleteAsync(string stagedMembershipRecordId)
        {
            var queryString3 = @"DELETE FROM StagedMembershipRecord WHERE StagedMembershipRecordId = @stagedMembershipRecordId;";
            return await _unitOfWork.StagedMembershipRecordRepository.ExecuteSqlAsync(queryString3, new { stagedMembershipRecordId });
        }
    }
}
