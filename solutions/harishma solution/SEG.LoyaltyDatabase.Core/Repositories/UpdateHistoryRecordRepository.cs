using Microsoft.Extensions.Configuration;
using SEG.ApiService.Models.Database;
using System.Data;

namespace SEG.LoyaltyDatabase.Core.Repositories
{
    public class UpdateHistoryRecordRepository : BaseRepository<UpdateHistoryRecord>
    {
        public UpdateHistoryRecordRepository(IDbConnection dbConnection) : base(dbConnection)
        {
        }
    }
}