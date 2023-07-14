using Microsoft.Extensions.Configuration;
using SEG.ApiService.Models.Database;
using SEG.LoyaltyDatabase.Models;
using System.Data;

namespace SEG.LoyaltyDatabase.Core.Repositories
{
    public class EmailPreferenceRepository : BaseRepository<EmailPreference>
    {
        public EmailPreferenceRepository(IDbConnection dbConnection) : base(dbConnection)
        {
        }
    }
}