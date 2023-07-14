using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Dapper;
using SEG.ApiService.Models;
using SEG.ApiService.Models.CRC;
using SEG.LoyaltyDatabase.Core.Interfaces;

namespace SEG.LoyaltyDatabase.Core.Services
{
    public class CardRangeService : ICardRangeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CardRangeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<CardRange>> GetAsync(Expression<Func<CardRange, bool>> expression)
        {
            return await _unitOfWork.CardRangeRepository.GetAsync(expression);
        }

        public async Task<IEnumerable<CardRange>> GetAllAsync()
        {
            return await _unitOfWork.CardRangeRepository.GetAllAsync();
        }

        public async Task<string> GetGeneratedMemberIdAsync()
        {
            var sprocName = "[dbo].[sp_GenerateMemberID]";
            var sprocParams = new DynamicParameters();
            sprocParams.Add("@GeneratedMemberID", dbType: DbType.String, size: 20, direction: ParameterDirection.Output);
            var results = await _unitOfWork.CardRangeRepository.ExecuteStoredProcedureAsync(sprocName, sprocParams);
            return sprocParams.Get<string>("@GeneratedMemberID");
        }

        public async Task<string> GetGeneratedCardNumberAsync()
        {
            var sprocName = "[dbo].[usp_GenerateCardNumber]";
            var sprocParams = new DynamicParameters();
            sprocParams.Add("@GeneratedCardNumber", dbType: DbType.String, size: 16, direction: ParameterDirection.Output);
            var results = await _unitOfWork.CardRangeRepository.ExecuteStoredProcedureAsync(sprocName, sprocParams);
            return sprocParams.Get<string>("@GeneratedCardNumber");
        }

        public async Task<decimal> GetGeneratedCrcAsync(Banner banner)
        {
            var sprocName = "[dbo].[sp_GenerateCRC]";
            var sprocParams = new DynamicParameters();
            sprocParams.Add("@GeneratedCrc", dbType: DbType.Decimal, size: 18, direction: ParameterDirection.Output);
            sprocParams.Add("@banner", (int)banner, DbType.Int32);
            var results = await _unitOfWork.CardRangeRepository.ExecuteStoredProcedureAsync(sprocName, sprocParams);
            return sprocParams.Get<decimal>("@GeneratedCrc");
        }
    }
}