using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SEG.ApiService.Models.Database;
using SEG.LoyaltyDatabase.Core.Interfaces;

namespace SEG.LoyaltyDatabase.Core.Services
{
    public class AdHocSMSJobItemService : IAdHocSMSJobItemService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdHocSMSJobItemService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> UpdateAsync(AdHocSMSJobItem item)
        {
            return await _unitOfWork.AdHocSMSJobItemRepository.UpdateAsync(item);
        }

        public async Task<AdHocSMSJobItem> GetJobItemIdByJobIdAsync(Guid id)
        {
            var results = await _unitOfWork.AdHocSMSJobItemRepository.GetAsync(a => a.JobId == id);
            return results.FirstOrDefault();
        }

        public async Task<bool> IsJobItemCompletedByJobIdAsync(Guid id)
        {
            var results = await _unitOfWork.AdHocSMSJobItemRepository.GetAsync(a => a.JobId == id && a.Processed == true);
            return results.Any();
        }

        public async Task<IEnumerable<AdHocSMSJobItem>> GetJobItemsAsync(Expression<Func<AdHocSMSJobItem, bool>> expression)
        {
            return await _unitOfWork.AdHocSMSJobItemRepository.GetAsync(expression);
        }
    }
}