using Microsoft.Extensions.Configuration;
using SEG.ApiService.Models.Database;
using System.Data;

namespace SEG.LoyaltyDatabase.Core.Repositories
{
    public class StagedMembershipRecordRepository :  BaseRepository<StagedMembershipRecord>
    {
        public StagedMembershipRecordRepository(IDbConnection dbConnection) : base(dbConnection)
        {
        }
    }
}