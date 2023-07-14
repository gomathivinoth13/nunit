using System.Threading.Tasks;
using SEG.ApiService.Models.Database;
using SEG.LoyaltyDatabase.Models;

namespace SEG.LoyaltyDatabase.Core.Interfaces
{
    public interface IGoodwillAuditService
    {
        Task<bool> AddAsync(GoodwillAudit audit);
    }
}