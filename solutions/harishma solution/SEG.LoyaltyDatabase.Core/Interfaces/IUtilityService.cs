using SEG.ApiService.Models;
using SEG.ApiService.Models.CRC;
using SEG.ApiService.Models.Payload;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SEG.LoyaltyDatabase.Core.Interfaces
{
    public interface IUtilityService
    {
        Task<ValidationResponse> ValidateAsync(Banner banner, decimal cardNumber, CardRange bannerObj = null);
        Task<bool> IsPlentiCardInSegBinRangeAsync(decimal cardNUmber);
        Task<bool> DoesCustomerGetAutomaticDataRightsAsync(Customer customer);

    }
}
