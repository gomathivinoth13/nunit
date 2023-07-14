using System.Threading.Tasks;
using SEG.ApiService.Models.Database;
using SEG.LoyaltyDatabase.Models;

namespace SEG.LoyaltyDatabase.Core.Interfaces
{
    public interface ICustPhoneLookupService
    {
        //Task<CustPhoneLookup> FindAsync(string phoneNumber);
        Task<bool> AddAsync(CustPhoneLookup lookup);
    }
}