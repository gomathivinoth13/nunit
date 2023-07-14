using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SEG.ApiService.Models;
using SEG.ApiService.Models.CRC;

namespace SEG.LoyaltyDatabase.Core.Interfaces
{
    public interface ICardRangeService
    {
        Task<IEnumerable<CardRange>> GetAsync(Expression<Func<CardRange, bool>> expression);
        Task<IEnumerable<CardRange>> GetAllAsync();
        Task<string> GetGeneratedMemberIdAsync();
        Task<string> GetGeneratedCardNumberAsync();
        Task<decimal> GetGeneratedCrcAsync(Banner banner);
    }
}