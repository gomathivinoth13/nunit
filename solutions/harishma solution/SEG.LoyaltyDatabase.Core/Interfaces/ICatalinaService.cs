using System.Collections.Generic;
using System.Threading.Tasks;
using SEG.ApiService.Models.Catalina;

namespace SEG.LoyaltyDatabase.Core.Interfaces
{
    public interface ICatalinaService
    {
        Task<string> RegisterCatalinaWinnerAsync(CatalinaWinners catalinaWinners);

        /// <summary>   Gets all register catalina winner. </summary>
        ///
        /// <remarks>   Mcdand, 8/9/2018. </remarks>
        ///
        /// <param name="catalinaWinners">  The catalina winners. </param>
        ///
        /// <returns>   An asynchronous result that yields all register catalina winner. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        Task<List<CatalinaWinners>> GetAllRegisterCatalinaWinner(CatalinaWinners catalinaWinners);
    }
}