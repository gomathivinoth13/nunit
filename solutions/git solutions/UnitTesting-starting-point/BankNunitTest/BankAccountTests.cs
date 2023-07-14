using Bank;
using NUnit.Framework;

namespace BankNunitTests
{
    public class BankAccountTests
    {
        private BankAccount account;


        [SetUp]
        public void Setup()
        {
         
            account = new BankAccount(1000);

        }
        [Test]
        public void Adding_Funds_Update_Balance()
        {

           
            account.Add(500);
            Assert.AreEqual(1500, account.Balance);
        }
        [Test]
        public void witdraw_Funds_Update_Balance()
        {

           
            account.Withdraw(500);
            Assert.AreEqual(500, account.Balance);
        }



        [Test]
        public void transferring_funds_account_updates()
        {

            
            var otheraccount = new BankAccount();
            account.TransferFundsTo(otheraccount,500);
            Assert.AreEqual(500, account.Balance);
            Assert.AreEqual(500, otheraccount.Balance);
        }
    }
}