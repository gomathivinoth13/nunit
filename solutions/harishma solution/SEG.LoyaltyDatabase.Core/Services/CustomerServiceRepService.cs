using System;
using System.Linq;
using System.Threading.Tasks;
using SEG.ApiService.Models.Payload;
using SEG.LoyaltyDatabase.Core.Interfaces;

namespace SEG.LoyaltyDatabase.Core.Services
{
    public class CustomerServiceRepService : ICustomerServiceRepService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly string _sql;

        public CustomerServiceRepService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _sql = "INSERT INTO Audit.CustomerServiceRep (UserId, FirstName, LastName) VALUES(@UserId, @FirstName, @LastName);";
        }

        public async Task<CustomerServiceRep> GetByUserIdAsync(string id)
        {
            var results = await _unitOfWork.CustomerServiceRepRepository.GetAsync(c => c.UserId == id);
            return results.FirstOrDefault();
        }

        public async Task<bool> AddAsync(CustomerServiceRep rep)
        {
            return await _unitOfWork.CustomerServiceRepRepository.ExecuteSqlAsync(_sql, rep);
        }

        public bool Add(ref CustomerServiceRep rep)
        {
            return _unitOfWork.CustomerServiceRepRepository.ExecuteSql(_sql, rep);
        }
    }
}