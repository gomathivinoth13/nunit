using SEG.ApiService.Models.Mobile;
using System.Data;

namespace SEG.LoyaltyDatabase.Core.Repositories
{
    public class MobileUsageAuditRecordRepository : BaseRepository<MobileUsageAuditRecord>
    {
        public MobileUsageAuditRecordRepository(IDbConnection dbConnection) : base(dbConnection)
        {
        }
    }
}