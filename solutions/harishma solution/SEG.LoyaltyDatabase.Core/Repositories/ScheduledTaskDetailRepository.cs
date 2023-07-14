using Microsoft.Extensions.Configuration;
using SEG.ApiService.Models.Database;
using SEG.LoyaltyDatabase.Models;
using System.Data;

namespace SEG.LoyaltyDatabase.Core.Repositories
{
    public class ScheduledTaskDetailRepository : BaseRepository<ScheduledTaskDetail>
    {
        public ScheduledTaskDetailRepository(IDbConnection dbConnection) : base(dbConnection)
        {
        }
    }
}