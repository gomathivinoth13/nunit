using System.Threading.Tasks;
using SEG.ApiService.Models.Payload;

namespace SEG.LoyaltyDatabase.Core.Interfaces
{
    public interface ICustomerPointTransactionService
    {
        Task<bool> Add(CustomerPointTransaction transaction);
    }
}