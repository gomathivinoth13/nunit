using System.Threading.Tasks;
using SEG.ApiService.Models.Queueing;

namespace SEG.LoyaltyDatabase.Core.Interfaces
{
    public interface IErrorQueueService
    {
        Task<bool> AddErrorTaskToDeadQueueAsync(ErrorQueueTask errorQueueTask);
    }
}