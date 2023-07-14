using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using SEG.ApiService.Models.Catalina;
using SEG.LoyaltyDatabase.Core.Interfaces;

namespace SEG.LoyaltyDatabase.Core.Services
{
    public class CatalinaService : ICatalinaService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CatalinaService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string> RegisterCatalinaWinnerAsync(CatalinaWinners catalinaWinners)
        {
            bool InsertWinner = true;
            var results = await _unitOfWork.CatalinaWinnersRepository.GetAsync(c => c.PhoneNumber == catalinaWinners.PhoneNumber && c.EmailAddress == catalinaWinners.EmailAddress);
            var entryResult = results.ToList();

            foreach (var r in entryResult)
            {
                if ((catalinaWinners.EntryTime - r.EntryTime).TotalDays < 1 && catalinaWinners.OfferCode.ToUpper().Trim() == r.OfferCode.ToUpper().Trim())
                {
                    InsertWinner = false;
                }
            }

            if (entryResult == null || InsertWinner == true)
            {
                await _unitOfWork.CatalinaWinnersRepository.InsertAsync(catalinaWinners, includePrimaryKey: true);
                return "Success";

            }
            else
            {
                return "Already Enrolled";
            }
        }

        public async Task<List<CatalinaWinners>> GetAllRegisterCatalinaWinner(CatalinaWinners catalinaWinners)
        {
            var queryString2 = @"INSERT INTO dbo.CatalinaWinners (FirstName, LastName, EmailAddress, PhoneNumber, TermsAccepted, School, MemberID, CRC_ID, EntryTime, OfferCode) VALUES (@FirstName, @LastName, @EmailAddress, @PhoneNumber, @TermsAccepted, @School, @MemberID, @CRC_ID, @EntryTime, @OfferCode);";
            var winners = await _unitOfWork.CatalinaWinnersRepository.ExecuteSqlAsync(queryString2, catalinaWinners);
            return winners.ToList();
        }
    }

}
