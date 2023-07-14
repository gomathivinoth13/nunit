using SEG.ApiService.Models.Database;
using SEG.LoyaltyDatabase.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SEG.LoyaltyDatabase.Core.Interfaces
{
    public interface ISMSTemplateService
    {
        Task<IEnumerable<SMSTemplate>> GetByNameAsync(string name);
        Task<IEnumerable<SMSTemplate>> GetTemplatesAsync();
    }
}