////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	customerdal.cs
//
// summary:	Implements the customerdal class
////////////////////////////////////////////////////////////////////////////////////////////////////

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SEG.AzureLoyaltyDatabase.DataAccess;
using System.Threading.Tasks;

namespace SEG.AzureLoyaltyDatabase.Tests
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   (Unit Test Class) a customer dal. </summary>
    ///
    /// <remarks>   Mcdand, 2/20/2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    [TestClass]
    public class CustomerDAL: TestsBase
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   (Unit Test Method) gets customer by member identifier. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <returns>   An asynchronous result. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        [TestMethod]
        public async Task GetCustomerByMemberID()
        {
            var customer = await SEG.AzureLoyaltyDatabase.AzureLoyaltyDatabaseManager.GetCustomerByMemberId("PB00000000002793487");
            var b = customer;

        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   (Unit Test Method) gets customers by email or phone. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <returns>   An asynchronous result. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        [TestMethod]
        public async Task GetCustomersByEmailOrPhone()
        {
            var customer = await SEG.AzureLoyaltyDatabase.AzureLoyaltyDatabaseManager.GetCustomerByEmailOrPhoneNumber("9045559246");
            var b = customer;

        }



        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   (Unit Test Method) gets promos filtered by date in V2
        ///
        /// <remarks>   gsukhorukov, 2022-06-09 </remarks>
        ///
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        [TestMethod]
        public async Task GetCustomerRewardPromo()
        {
            var promos = await SEG.AzureLoyaltyDatabase.AzureLoyaltyDatabaseManager.GetCustomerPromoSlidersV2();
            var b = promos;

        }


    }
}
