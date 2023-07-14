using Microsoft.Extensions.Configuration;
using SEG.ApiService.Models.Database;
using System.Data;

namespace SEG.LoyaltyDatabase.Core.Repositories
{
    public class ScheduledProcessBatchUpdateRepository : BaseRepository<ScheduledProcessBatchUpdate>
    {
        public ScheduledProcessBatchUpdateRepository(IDbConnection dbConnection) : base(dbConnection)
        {
        }
    }
}