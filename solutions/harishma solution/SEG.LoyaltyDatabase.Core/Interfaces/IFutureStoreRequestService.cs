using System.Collections.Generic;
using System.Threading.Tasks;
using SEG.ApiService.Models.FutureStore;

namespace SEG.LoyaltyDatabase.Core.Interfaces
{
    public interface IFutureStoreRequestService
    {
        Task<string> FutureStoreRequestAsync(FutureStoreRequest futureStore);
        Task<List<FutureStoreRequest>> GetFutureStoreRequests(FutureStoreRequest futureStoreRequest);
    }
}