using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SEG.ApiService.Models.Database;

namespace SEG.LoyaltyDatabase.Core.Interfaces
{
    public interface IAdHocSMSJobItemService
    {
        Task<bool> UpdateAsync(AdHocSMSJobItem item);
        Task<AdHocSMSJobItem> GetJobItemIdByJobIdAsync(Guid id);
        Task<bool> IsJobItemCompletedByJobIdAsync(Guid id);
        Task<IEnumerable<AdHocSMSJobItem>> GetJobItemsAsync(Expression<Func<AdHocSMSJobItem, bool>> expression);
    }
}