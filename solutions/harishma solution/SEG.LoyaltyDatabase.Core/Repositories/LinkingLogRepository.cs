using Microsoft.Extensions.Configuration;
using SEG.ApiService.Models.Database;
using System.Data;

namespace SEG.LoyaltyDatabase.Core.Repositories
{
    public class LinkingLogRepository : BaseRepository<LinkingLog>
    {
        public LinkingLogRepository(IDbConnection dbConnection) : base(dbConnection)
        {
        }
    }
}