using System;
using System.Threading.Tasks;
using SEG.ApiService.Models.Payload;

namespace SEG.LoyaltyDatabase.Core.Interfaces
{
    public interface ICustomerServiceRepService
    {
        Task<CustomerServiceRep> GetByUserIdAsync(string id);
        Task<bool> AddAsync(CustomerServiceRep rep);
        bool Add(ref CustomerServiceRep rep);
    }
}