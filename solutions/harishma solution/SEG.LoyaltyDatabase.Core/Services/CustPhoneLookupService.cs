using System;
using System.Linq;
using System.Threading.Tasks;
using SEG.LoyaltyDatabase.Core.Interfaces;
using SEG.LoyaltyDatabase.Models;

namespace SEG.LoyaltyDatabase.Core.Services
{
    public class CustPhoneLookupService : ICustPhoneLookupService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CustPhoneLookupService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //public async Task<CustPhoneLookup> FindAsync(string phoneNumber)
        //{
        //    var tableName = _unitOfWork.CustPhoneLookupRepository.TableName;
        //    var sql = $"SELECT * FROM {tableName} WHERE PhoneNumber=@PhoneNumber";
        //   var result = await _unitOfWork.CustPhoneLookupRepository.GetAsync<CustPhoneLookup>(sql, phoneNumber);
        //   var custPhoneLookups = result as CustPhoneLookup[] ?? result.ToArray();
        //   if (custPhoneLookups.Length > 1) throw new Exception("More than one result with same phone number found.");
        //   return custPhoneLookups.FirstOrDefault();
        //}

        public async Task<bool> AddAsync(CustPhoneLookup lookup)
        {
            return await _unitOfWork.CustPhoneLookupRepository.InsertAsync(lookup);
        }
    }
}