using System.Threading.Tasks;
using SEG.LoyaltyDatabase.Core.Interfaces;
using SEG.ApiService.Models.Database;
using SEG.LoyaltyDatabase.Models;

namespace SEG.LoyaltyDatabase.Core.Services
{
    public class GoodwillAuditService : IGoodwillAuditService
    {
        private readonly IUnitOfWork _unitOfWork;

        public GoodwillAuditService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> AddAsync(GoodwillAudit audit)
        {
            var sql = "INSERT INTO Audit.GoodWill (CrcId, ChainId, StoreNumber, ReceiptNumber, TotalPoints, CreateDateTime, CreateUser, LastUpdateDateTime, LastUpdateUser) Values (@CrcId, @ChainId, @StoreNumber, @ReceiptNumber, @TotalPoints, @CreateDateTime, @CreateUser, @LastUpdateDateTime, @LastUpdateUser);";
            return await _unitOfWork.GoodwillAuditRepository.ExecuteSqlAsync(sql, audit);
        }
    }
}