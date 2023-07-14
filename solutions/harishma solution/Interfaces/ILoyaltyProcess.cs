using System.Threading.Tasks;
using SEG.ApiService.Models;

namespace SEG.LoyaltyService.Process.Core
{
    public interface ILoyaltyProcess
    {
        /// <summary>   Gets member alias. </summary>
        ///
        /// <remarks>   Mcdand, 2/19/2018. </remarks>
        ///
        /// <param name="crcId">    Identifier for the CRC. </param>
        ///
        /// <returns>   An asynchronous result that yields the member alias. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        Task<MemberAlias> GetMemberAlias(string crcId);
    }
}