////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	LoyaltyProcess.cs
//
// summary:	Implements the loyalty process class
////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Linq;
using System.Threading.Tasks;
using SEG.ApiService.Models;
using SEG.ApiService.Models.Enum;
using SEG.LoyaltyService.Process.Core.Interfaces;

namespace SEG.LoyaltyService.Process.Core
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   A loyalty process. </summary>
    ///
    /// <remarks>   Mcdand, 2/19/2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public class LoyaltyProcess : ILoyaltyProcess
    {
        private readonly ICustomerProcess _customerProcess;

        public LoyaltyProcess(ICustomerProcess customerProcess)
        {
            _customerProcess = customerProcess;
        }

        #region Public Methods

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets member alias. </summary>
        ///
        /// <remarks>   Mcdand, 2/19/2018. </remarks>
        ///
        /// <param name="crcId">    Identifier for the CRC. </param>
        ///
        /// <returns>   An asynchronous result that yields the member alias. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public async Task<MemberAlias> GetMemberAlias(string crcId)
        {
            MemberAlias memberAlias = null;

            CustomerSearchResponse response = await _customerProcess.CustomerSearchAsync(new CustomerSearchRequest() { CrcId = crcId });
            if (response != null && response.IsSuccessful)
            {
                CustomerV2 customer = response.Customers.FirstOrDefault();
                if (customer != null)
                {
                    if (customer != null && customer.CustomerAlias != null && customer.CustomerAlias.Any())
                    {
                        memberAlias = customer.CustomerAlias.Where(a => a.AliasStatus != (short)AliasStatusType.Cancelled).FirstOrDefault();
                    }
                }
            }

            return memberAlias;
        }

        #endregion RewardClub
    }
}