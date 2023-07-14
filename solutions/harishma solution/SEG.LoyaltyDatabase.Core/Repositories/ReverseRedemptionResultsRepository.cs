using Microsoft.Extensions.Configuration;
using SEG.ApiService.Models.Omni;
using System.Data;

namespace SEG.LoyaltyDatabase.Core.Repositories
{
    public class ReverseRedemptionResultsRepository : BaseRepository<ReverseRedemptionResults>
    {
        public ReverseRedemptionResultsRepository(IDbConnection dbConnection) : base(dbConnection)
        {
        }
    }
}