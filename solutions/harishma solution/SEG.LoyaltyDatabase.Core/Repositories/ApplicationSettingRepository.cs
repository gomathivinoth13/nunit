using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Microsoft.Extensions.Configuration;
using SEG.ApiService.Models.Database;
using SEG.LoyaltyDatabase.Models;

namespace SEG.LoyaltyDatabase.Core.Repositories
{
    public class ApplicationSettingRepository : BaseRepository<ApplicationSetting>
    {
        public ApplicationSettingRepository(IDbConnection dbConnection) : base(dbConnection)
        {
        }
    }
}
