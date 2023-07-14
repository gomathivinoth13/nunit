using SEG.ApiService.Models.Database;
using SEG.LoyaltyDatabase.Models;
using System.Threading.Tasks;

namespace SEG.LoyaltyDatabase.Core.Interfaces
{
    public interface ISMSHistoryRecordsService
    {
        SMSHistory Add(ref SMSHistory history);
        Task<bool> AddAsync(SMSHistory history);
    }
}