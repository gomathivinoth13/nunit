﻿using Microsoft.Extensions.Configuration;
using SEG.ApiService.Models.Database;
using SEG.LoyaltyDatabase.Models;
using System.Data;

namespace SEG.LoyaltyDatabase.Core.Repositories
{
    public class SMSTemplateRepository : BaseRepository<SMSTemplate>
    {
        public SMSTemplateRepository(IDbConnection dbConnection) : base(dbConnection)
        {
        }
    }
}