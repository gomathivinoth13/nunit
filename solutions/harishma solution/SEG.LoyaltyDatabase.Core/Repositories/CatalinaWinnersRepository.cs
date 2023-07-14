using Microsoft.Extensions.Configuration;
using SEG.ApiService.Models.Catalina;
using System.Data;

namespace SEG.LoyaltyDatabase.Core.Repositories
{
    public class CatalinaWinnersRepository : BaseRepository<CatalinaWinners>
    {
        public CatalinaWinnersRepository(IDbConnection dbConnection) : base(dbConnection)
        {
        }
    }
}