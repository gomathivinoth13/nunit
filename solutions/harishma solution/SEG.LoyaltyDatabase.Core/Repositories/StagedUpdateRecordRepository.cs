using Microsoft.Extensions.Configuration;
using SEG.ApiService.Models.Database;
using System.Data;

namespace SEG.LoyaltyDatabase.Core.Repositories
{
    public class StagedUpdateRecordRepository : BaseRepository<StagedUpdateRecord>
    {
        public StagedUpdateRecordRepository(IDbConnection dbConnection) : base(dbConnection)
        {
        }
    }
}