using System.Collections.Generic;
using System.Threading.Tasks;
using SEG.ApiService.Models.Database;

namespace SEG.LoyaltyDatabase.Core.Interfaces
{
    public interface IStagedMembershipRecordService
    {
        Task<List<StagedMembershipRecord>> GetStagedMembershipRecordsByMemberIdAsync(string memberId);

        /// <summary>   Inserts the given record. </summary>
        ///
        /// <remarks>   Mcdand, 2/19/2018. </remarks>
        ///
        /// <param name="record">   The record. </param>
        ///
        /// <returns>   True if it succeeds, false if it fails. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        Task<bool> InsertAsync(StagedMembershipRecord record);

        /// <summary>   Deletes the given stagedMembershipRecordId. </summary>
        ///
        /// <remarks>   Mcdand, 2/19/2018. </remarks>
        ///
        /// <param name="stagedMembershipRecordId"> The staged membership record Identifier to delete. </param>
        ///
        /// <returns>   True if it succeeds, false if it fails. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        Task<bool> DeleteAsync(string stagedMembershipRecordId);
    }
}