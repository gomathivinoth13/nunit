using Microsoft.Extensions.Configuration;
using SEG.ApiService.Models.Database;
using SEG.LoyaltyDatabase.Models;
using System.Data;

namespace SEG.LoyaltyDatabase.Core.Repositories
{
    public class DeadTaskQueueRepository : BaseRepository<DeadTaskQueue>
    {
        public DeadTaskQueueRepository(IDbConnection dbConnection) : base(dbConnection)
        {
        }
    }
}