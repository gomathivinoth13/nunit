using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using SEG.ApiService.Models.Mobile;
using SEG.LoyaltyDatabase.Core.Interfaces;

namespace SEG.LoyaltyDatabase.Core.Services
{
    public class MobileApplicationUsageService : IMobileApplicationUsageService
    {
        private readonly IUnitOfWork _unitOfWork;
        
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Saves a mobile application audit record asynchronous. </summary>
        /// 
        /// <remarks>   Mcdand, 8/9/2018. </remarks>
        /// 
        /// <param name="record">   The record. </param>
        /// <param name="unitOfWork"></param>
        /// <returns>   An asynchronous result. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public MobileApplicationUsageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> SaveMobileAppAuditRecordAsync(MobileUsageAuditRecord record)
        {
            return await _unitOfWork.MobileUsageAuditRecordRepository.UpdateAsync(record);
        }
    }
}
