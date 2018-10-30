using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BankAppConsole;
using System.Configuration;
using System.Data.OleDb;
using System.Data;

namespace BankAppTests 

{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestDepositWithSavings()
        {
            string AccNum = "1234567890";
            Account<SavingsAccount> account = new Account<SavingsAccount>(BankDatabaseEngine.GetAccountName(AccNum), AccNum, 
                BankDatabaseEngine.GetAccountBalance(AccNum));
            double creditAmount = 10000;
            
            double expected = BankDatabaseEngine.GetAccountBalance(AccNum) + creditAmount;
            double actual = account.makeDeposit(AccNum, creditAmount);
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void TestCurrentCreditAccount()
        {
            string AccNum = "5831314992";
            Account<CurrentAccount> account = new Account<CurrentAccount>(BankDatabaseEngine.GetAccountName(AccNum), AccNum,
                BankDatabaseEngine.GetAccountBalance(AccNum));
            double creditAmount = 10000;

            double expected = BankDatabaseEngine.GetAccountBalance(AccNum) + creditAmount;
            Account<CurrentAccount>.CreditAccount(AccNum, creditAmount);
            Assert.AreEqual(expected, BankDatabaseEngine.GetAccountBalance(AccNum));

        }

        [TestMethod]
        public void TestSavingsCreditAccount() 
        {
            string AccNum = "1234567890";
            Account<SavingsAccount> account = new Account<SavingsAccount>(BankDatabaseEngine.GetAccountName(AccNum), AccNum,
                BankDatabaseEngine.GetAccountBalance(AccNum));
            double creditAmount = 10000;

            double expected = BankDatabaseEngine.GetAccountBalance(AccNum) + creditAmount;
            Account<SavingsAccount>.CreditAccount(AccNum, creditAmount);
            Assert.AreEqual(expected, BankDatabaseEngine.GetAccountBalance(AccNum));
        }

        [TestMethod]
        public void TestCurrentDebitAccount() 
        {
            string AccNum = "5831314992";
            Account<CurrentAccount> account = new Account<CurrentAccount>(BankDatabaseEngine.GetAccountName(AccNum),
                AccNum, BankDatabaseEngine.GetAccountBalance(AccNum));
            double debitAmount = 10000;

            double expected = BankDatabaseEngine.GetAccountBalance(AccNum) - debitAmount;
            Account<CurrentAccount>.DebitAccount(AccNum, debitAmount);
            Assert.AreEqual(expected, BankDatabaseEngine.GetAccountBalance(AccNum));
        }

        [TestMethod]
        public void TestSavingsDebitAccount() 
        {
            string AccNum = "1234567890";
            Account<SavingsAccount> account = new Account<SavingsAccount>(BankDatabaseEngine.GetAccountName(AccNum)
                , AccNum, BankDatabaseEngine.GetAccountBalance(AccNum));
            double debitAmount = 10000;

            double expected = BankDatabaseEngine.GetAccountBalance(AccNum) - debitAmount;
            Account<SavingsAccount>.DebitAccount(AccNum, debitAmount);
            Assert.AreEqual(expected, BankDatabaseEngine.GetAccountBalance(AccNum));
        }

        [TestMethod]
        public void TestInterest(){

            double rate = 0.005;
            BankPolicy<SavingsAccount>.interest_rate = rate;
            string AccNum = "1234567890";
            Account<SavingsAccount> account = new Account<SavingsAccount>(BankDatabaseEngine.GetAccountName(AccNum)
                , AccNum, BankDatabaseEngine.GetAccountBalance(AccNum));
            double expected = BankDatabaseEngine.GetAccountBalance(AccNum) + 
                (rate * BankDatabaseEngine.GetAccountBalance(AccNum));
             account.AwardInterest(AccNum);
             double actual = BankDatabaseEngine.GetAccountBalance(AccNum);
             Assert.AreEqual(expected,actual );
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestInterestWithWrongAccountType() 
        {

            double rate = 0.045;
            BankPolicy<CurrentAccount>.interest_rate = rate;
            string AccNum = "5831314992";
            Account<CurrentAccount> account = new Account<CurrentAccount>(BankDatabaseEngine.GetAccountName(AccNum)
                , AccNum, BankDatabaseEngine.GetAccountBalance(AccNum));
            double expected = BankDatabaseEngine.GetAccountBalance(AccNum) +
                (rate * BankDatabaseEngine.GetAccountBalance(AccNum));
            account.AwardInterest(AccNum);
            Assert.AreEqual(expected, BankDatabaseEngine.GetAccountBalance(AccNum));
        }

        [TestMethod]
        public void TestDepositWithCurrent() 
        {
            string AccNum = "5831314992";
            Account<CurrentAccount> account = new Account<CurrentAccount>(BankDatabaseEngine.GetAccountName(AccNum), AccNum,
                BankDatabaseEngine.GetAccountBalance(AccNum));
            double creditAmount = 10000;

            double expected = BankDatabaseEngine.GetAccountBalance(AccNum) + creditAmount;
            double actual = account.makeDeposit(AccNum, creditAmount);
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void TestWithdrawalWithCurrent() 
        {
            string AccNum = "5831314992";
            Account<CurrentAccount> account = new Account<CurrentAccount>(BankDatabaseEngine.GetAccountName(AccNum), AccNum,
                BankDatabaseEngine.GetAccountBalance(AccNum));
            double creditAmount = 10000;

            double expected = BankDatabaseEngine.GetAccountBalance(AccNum) - creditAmount;
            double actual = account.makeWithdrawal(AccNum, creditAmount);
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void TestWithrawalWithSavings() 
        {
            string AccNum = "1234567890";
            Account<SavingsAccount> account = new Account<SavingsAccount>(BankDatabaseEngine.GetAccountName(AccNum), AccNum,
                BankDatabaseEngine.GetAccountBalance(AccNum));
            double debitAmount = 10000;

            double expected = BankDatabaseEngine.GetAccountBalance(AccNum) - debitAmount;
            double actual = account.makeWithdrawal(AccNum, debitAmount);
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void TestCurrentOverdraftWithdrawal()
        {
            BankPolicy<CurrentAccount>.minimumBalance = 1000;
            string AccNum = "7832463793";
            Account<CurrentAccount> account = new Account<CurrentAccount>(BankDatabaseEngine.GetAccountName(AccNum), AccNum,
                BankDatabaseEngine.GetAccountBalance(AccNum));
            double withdrawAmount = 900000000;
            double expected = BankDatabaseEngine.GetAccountBalance(AccNum) - withdrawAmount;
            double actual = CurrentAccount.overdraftWithdraw(account, AccNum, withdrawAmount);

            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInvalidAccountDepositWithSavings()
        {
            Account<SavingsAccount> account = new Account<SavingsAccount>("Customer", "000000", 12000.00);

            double actual = account.makeDeposit("000000", 6000);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInvalidAccountWithdrawalWithSavings()
        {

            Account<SavingsAccount> account = new Account<SavingsAccount>("Customer", "000000", 12000.00);

            double actual = account.makeWithdrawal("000000", 6000);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInvalidAccountDepositWithCurrent()
        {

            Account<CurrentAccount> account = new Account<CurrentAccount>("Customer", "00000", 22000.00);

            double actual = account.makeDeposit("00000", 10000.00);
         }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInvalidAccountWithdrawalWithCurrent()
        {

            Account<CurrentAccount> account = new Account<CurrentAccount>("Customer", "000000", 120000.00);

            double actual = account.makeWithdrawal("000000", 60000);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WithdrawNegNumFromSavings()
        {

            Account<SavingsAccount> account = new Account<SavingsAccount>("Customer", "000000", 12000.00);

            account.makeWithdrawal("000000", -900);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WithdrawMoreThanSavingsBalance()
        {

            Account<SavingsAccount> account = new Account<SavingsAccount>("Customer", "000000", 12000.00);

            account.makeWithdrawal("000000", 19000);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WithdrawIntoReservedBalance()
        {
            BankPolicy<SavingsAccount>.minimumBalance = 1000;
            Account<SavingsAccount> account = new Account<SavingsAccount>("Customer", "000000", 11000.00);

            account.makeWithdrawal("000000", 11500);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DepositNegAmountToSavings()
        {

            Account<SavingsAccount> account = new Account<SavingsAccount>("Customer", "000000", 11000.00);

            account.makeDeposit("000000", -2500);

        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WithdrawNegNumFromCurrent()
        {

            Account<CurrentAccount> account = new Account<CurrentAccount>("Customer", "000000", 9000.50);
            account.makeWithdrawal("000000", -900);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WithdrawMoreThanCurrentBalance()
        {

            Account<CurrentAccount> account = new Account<CurrentAccount>("Customer", "000000", 110080.00);

            account.makeWithdrawal("000000", 130000);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WithdrawIntoMinimumBalance()
        {

            BankPolicy<CurrentAccount>.minimumBalance = 5000;
            Account<CurrentAccount> account = new Account<CurrentAccount>("Customer", "000000", 11000);

            account.makeWithdrawal("000000", 13500);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DepositNegAmountToCurrent()
        {
            Account<CurrentAccount> account = new Account<CurrentAccount>("Customer", "000000", 800);

            account.makeDeposit("000000", -1500);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void LessThanMinimumCurrentBalance()
        {
            BankPolicy<CurrentAccount>.minimumBalance = 5000;

            Account<CurrentAccount> account = new Account<CurrentAccount>("Customer", "000000", 1000);


        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void LessThanMinimumSavingsBalance()
        {
            BankPolicy<SavingsAccount>.minimumBalance = 1000;

            Account<SavingsAccount> account = new Account<SavingsAccount>("Customer", "000000", 500);


        }

        [TestMethod]
        [ExpectedException(typeof(PlatformNotSupportedException))]
        public void OpenWithInvalidAccountType()
        {
            Account<Object> account = new Account<Object>("Customer", "000000", 1000);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInvalidAccNumberOverdraftWithdrawal()
        {
            BankPolicy<CurrentAccount>.minimumBalance = 1000;
            Account<CurrentAccount> account = new Account<CurrentAccount>("Customer", "000000", 1200.00);

            double actual = CurrentAccount.overdraftWithdraw(account, "000000", 20000.00);
          
        }
       
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestCurrentOverdraftWithdrawalWithUnsupportedAmount()
        {

            Account<CurrentAccount> account = new Account<CurrentAccount>("Customer", "000000", 120000.00);

            CurrentAccount.overdraftWithdraw(account, "000000", 20000.00);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestTransactionWithWrongAccountNumber()
        {

            Account<CurrentAccount> account = new Account<CurrentAccount>("Customer", "011110", 120000.00);

            CurrentAccount.overdraftWithdraw(account, "011111", 20000.00);

        }

        [TestMethod]
        public void TestRandomStringGenerator()
        {
            string random = AccountNumberGenerator.GenerateUniqueString(10);
            Assert.IsTrue(!"[^a-zA-Z]".Contains(random));
        }

    }
}