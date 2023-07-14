using Microsoft.Extensions.Configuration;
using SEG.ApiService.Models.Database;
using System.Data;

namespace SEG.LoyaltyDatabase.Core.Repositories
{
    public class AdhocSMSJobRepository :  BaseRepository<AdhocSMSJob>
    {
        public AdhocSMSJobRepository(IDbConnection dbConnection) : base(dbConnection)
        {
        }
    }
}