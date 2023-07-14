using Microsoft.Extensions.Configuration;
using SEG.ApiService.Models.Database;
using SEG.LoyaltyDatabase.Models;
using System.Data;

namespace SEG.LoyaltyDatabase.Core.Repositories
{
    public class EmailPreference_TypeRepository : BaseRepository<EmailPreference_Type>
    {
        public EmailPreference_TypeRepository(IDbConnection dbConnection) : base(dbConnection)
        {
        }
    }
}