using System.Collections.Generic;
using System.Threading.Tasks;
using SEG.LoyaltyDatabase.Core.Interfaces;
using SEG.ApiService.Models.Database;
using SEG.LoyaltyDatabase.Models;

namespace SEG.LoyaltyDatabase.Core.Services
{
    public class ApplicationSettingService : IApplicationSettingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ApplicationSettingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task DeleteApplicationSettingAsync(string key)
        {
            var sql = "DELETE FROM ApplicationSetting WHERE [Key]=@key";
            await _unitOfWork.ApplicationSettingRepository.ExecuteSqlAsync(sql, new { key = key });
        }

        public async Task<IEnumerable<ApplicationSetting>> GetApplicationSettingsAsync(string key)
        {
            var sql = $"SELECT * FROM [ApplicationSetting] WHERE [Key] LIKE '{ key }%'";
            return await _unitOfWork.ApplicationSettingRepository.ExecuteSqlAsync(sql);
        }

        public async Task<IEnumerable<ApplicationSetting>> GetApplicationSettingsAsync()
        {
            return await _unitOfWork.ApplicationSettingRepository.GetAllAsync();
        }

        public async Task<bool> SaveApplicationSettingAsync(ApplicationSetting setting)
        {
            return await _unitOfWork.ApplicationSettingRepository.InsertAsync(setting);
        }

        public async Task<bool> UpsertApplicationSetting(ApplicationSetting setting)
        {
            var result = await _unitOfWork.ApplicationSettingRepository.GetAsync(a => a.Key == setting.Key);
            if (result == null)
            {
                return await SaveApplicationSettingAsync(setting);
            }
            else
            {
                return await _unitOfWork.ApplicationSettingRepository.UpdateAsync(setting);
            }
        }
    }
}