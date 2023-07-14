using SEG.ApiService.Models.Mobile;
using System.Threading.Tasks;

namespace SEG.LoyaltyDatabase.Core.Interfaces
{
    public interface IMobileApplicationUsageService
    {
        Task<bool> SaveMobileAppAuditRecordAsync(MobileUsageAuditRecord record);
    }
}