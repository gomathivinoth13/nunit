using System;
using System.Threading.Tasks;
using SEG.LoyaltyDatabase.Core.Interfaces;
using SEG.ApiService.Models.Database;
using SEG.LoyaltyDatabase.Models;

namespace SEG.LoyaltyDatabase.Core.Services
{
    public class SMSHistoryRecordsService : ISMSHistoryRecordsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SMSHistoryRecordsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public SMSHistory Add(ref SMSHistory history)
        {
            var isAdded = _unitOfWork.SMSHistoryRepository.Insert(ref history, false, true);
            if (!isAdded) throw new Exception($"Error saving SMS History record id= { history.Id }");
            return history;
        }

        public async Task<bool> AddAsync(SMSHistory history)
        {
            var isAdded = await _unitOfWork.SMSHistoryRepository.InsertAsync(history, false, true);
            if (!isAdded) throw new Exception($"Error saving SMS History record id= { history.Id }");
            return isAdded;
        }
    }
}