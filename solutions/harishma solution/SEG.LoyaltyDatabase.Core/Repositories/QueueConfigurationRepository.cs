using Microsoft.Extensions.Configuration;
using SEG.ApiService.Models.Database;
using System.Data;

namespace SEG.LoyaltyDatabase.Core.Repositories
{
    public class QueueConfigurationRepository : BaseRepository<QueueConfiguration>
    {
        public QueueConfigurationRepository(IDbConnection dbConnection) : base(dbConnection)
        {
        }
    }
}