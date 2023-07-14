using System.Threading.Tasks;
using SEG.ApiService.Models.Payload;
using SEG.LoyaltyDatabase.Core.Interfaces;

namespace SEG.LoyaltyDatabase.Core.Services
{
    public class CustomerPointTransactionService : ICustomerPointTransactionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustomerPointTransactionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Add(CustomerPointTransaction transaction)
        {
            return await _unitOfWork.CustomerPointTransactionRepository.InsertAsync(transaction, includePrimaryKey: true);
        }
    }
}