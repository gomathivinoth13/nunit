using Microsoft.Extensions.Configuration;
using SEG.ApiService.Models.Database;
using System.Data;

namespace SEG.LoyaltyDatabase.Core.Repositories
{
    public class AdHocSMSJobItemRepository : BaseRepository<AdHocSMSJobItem>
    {
        public AdHocSMSJobItemRepository(IDbConnection dbConnection) : base(dbConnection)
        {
        }
    }
}