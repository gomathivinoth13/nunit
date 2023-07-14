using System.Collections.Generic;
using System.Threading.Tasks;
using SEG.LoyaltyDatabase.Models;

namespace SEG.LoyaltyDatabase.Core.Interfaces
{
    public interface IApplicationSettingService
    {
        Task<IEnumerable<ApplicationSetting>> GetApplicationSettingsAsync();
        Task<IEnumerable<ApplicationSetting>> GetApplicationSettingsAsync(string key);
        Task<bool> SaveApplicationSettingAsync(ApplicationSetting setting);
        Task DeleteApplicationSettingAsync(string key);
        Task<bool> UpsertApplicationSetting(ApplicationSetting setting);
    }
}