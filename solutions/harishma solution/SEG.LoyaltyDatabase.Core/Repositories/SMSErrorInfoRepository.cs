using Microsoft.Extensions.Configuration;
using SEG.ApiService.Models.Database;
using SEG.LoyaltyDatabase.Models;
using System.Data;

namespace SEG.LoyaltyDatabase.Core.Repositories
{
    public class SMSErrorInfoRepository : BaseRepository<SMSErrorInfo>
    {
        public SMSErrorInfoRepository(IDbConnection dbConnection) : base(dbConnection)
        {
        }
    }
}