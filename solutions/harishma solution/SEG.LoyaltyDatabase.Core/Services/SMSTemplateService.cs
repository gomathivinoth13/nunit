using System.Collections.Generic;
using System.Threading.Tasks;
using SEG.LoyaltyDatabase.Core.Interfaces;
using SEG.ApiService.Models.Database;
using SEG.LoyaltyDatabase.Models;

namespace SEG.LoyaltyDatabase.Core.Services
{
    public class SMSTemplateService : ISMSTemplateService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SMSTemplateService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<SMSTemplate>> GetByNameAsync(string name)
        {
            var results = await _unitOfWork.SMSTemplateRepository.GetAsync(t => t.Name == name);
            return results;
        }

        public async Task<IEnumerable<SMSTemplate>> GetTemplatesAsync()
        {
            return await _unitOfWork.SMSTemplateRepository.GetAllAsync();
        }
    }
}