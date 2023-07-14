using Microsoft.Extensions.Configuration;
using SEG.ApiService.Models.Payload;
using System.Data;

namespace SEG.LoyaltyDatabase.Core.Repositories
{
    public class CustomerPointTransactionRepository : BaseRepository<CustomerPointTransaction>
    {
        public CustomerPointTransactionRepository(IDbConnection dbConnection) : base(dbConnection)
        {
        }
    }
}