using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Microsoft.Extensions.Configuration;
using SEG.ApiService.Models.CRC;

namespace SEG.LoyaltyDatabase.Core.Repositories
{
    public class CardRangeRepository : BaseRepository<CardRange>
    {
        public CardRangeRepository(IDbConnection dbConnection) : base(dbConnection)
        {
        }
    }
}
