using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BankAppConsole;

namespace TestBankApp
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestDepositWithSavings()
        {
            string AccNum = "1234567890";
            Account account = new SavingsAccount(BankDatabaseEngine.GetAccountName(AccNum), AccNum,
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
            Account account = new CurrentAccount(BankDatabaseEngine.GetAccountName(AccNum), AccNum,
                BankDatabaseEngine.GetAccountBalance(AccNum));
            double creditAmount = 10000;

            double expected = BankDatabaseEngine.GetAccountBalance(AccNum) + creditAmount;
            Teller.CreditAccount(AccNum, creditAmount);
            Assert.AreEqual(expected, BankDatabaseEngine.GetAccountBalance(AccNum));

        }

        [TestMethod]
        public void TestSavingsCreditAccount()
        {
            string AccNum = "1234567890";
            Account account = new SavingsAccount(BankDatabaseEngine.GetAccountName(AccNum), AccNum,
                BankDatabaseEngine.GetAccountBalance(AccNum));
            double creditAmount = 10000;

            double expected = BankDatabaseEngine.GetAccountBalance(AccNum) + creditAmount;
            Teller.CreditAccount(AccNum, creditAmount);
            Assert.AreEqual(expected, BankDatabaseEngine.GetAccountBalance(AccNum));
        }

        [TestMethod]
        public void TestCurrentDebitAccount()
        {
            string AccNum = "5831314992";
            Account account = new CurrentAccount(BankDatabaseEngine.GetAccountName(AccNum),
                AccNum, BankDatabaseEngine.GetAccountBalance(AccNum));
            double debitAmount = 10000;

            double expected = BankDatabaseEngine.GetAccountBalance(AccNum) - debitAmount;
            Teller.DebitAccount(AccNum, debitAmount);
            Assert.AreEqual(expected, BankDatabaseEngine.GetAccountBalance(AccNum));
        }

        [TestMethod]
        public void TestSavingsDebitAccount()
        {
            string AccNum = "1234567890";
            Account account = new SavingsAccount(BankDatabaseEngine.GetAccountName(AccNum)
                , AccNum, BankDatabaseEngine.GetAccountBalance(AccNum));
            double debitAmount = 10000;

            double expected = BankDatabaseEngine.GetAccountBalance(AccNum) - debitAmount;
            Teller.DebitAccount(AccNum, debitAmount);
            Assert.AreEqual(expected, BankDatabaseEngine.GetAccountBalance(AccNum));
        }

        
        [TestMethod]
        public void TestDepositWithCurrent()
        {
            string AccNum = "5831314992";
            Account account = new CurrentAccount(BankDatabaseEngine.GetAccountName(AccNum), AccNum,
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
            Account account = new CurrentAccount(BankDatabaseEngine.GetAccountName(AccNum), AccNum,
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
            Account account = new SavingsAccount(BankDatabaseEngine.GetAccountName(AccNum), AccNum,
                BankDatabaseEngine.GetAccountBalance(AccNum));
            double debitAmount = 10000;

            double expected = BankDatabaseEngine.GetAccountBalance(AccNum) - debitAmount;
            double actual = account.makeWithdrawal(AccNum, debitAmount);
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void TestCurrentOverdraftWithdrawal()
        {
            BankPolicy.minimumcurrentBalance = 1000;
            string AccNum = "7832463793";
            CurrentAccount account = new CurrentAccount(BankDatabaseEngine.GetAccountName(AccNum), AccNum,
                BankDatabaseEngine.GetAccountBalance(AccNum));
            double withdrawAmount = 900000000;
            double expected = BankDatabaseEngine.GetAccountBalance(AccNum) - withdrawAmount;
            double actual = account.overdraftWithdraw(AccNum, withdrawAmount);

            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInvalidAccountDepositWithSavings()
        {
            Account account = new SavingsAccount("Customer", "000000", 12000.00);

            double actual = account.makeDeposit("000000", 6000);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInvalidAccountWithdrawalWithSavings()
        {

            Account account = new SavingsAccount("Customer", "000000", 12000.00);

            double actual = account.makeWithdrawal("000000", 6000);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInvalidAccountDepositWithCurrent()
        {

            Account account = new CurrentAccount("Customer", "00000", 22000.00);

            double actual = account.makeDeposit("00000", 10000.00);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInvalidAccountWithdrawalWithCurrent()
        {

            Account account = new CurrentAccount("Customer", "000000", 120000.00);

            double actual = account.makeWithdrawal("000000", 60000);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WithdrawNegNumFromSavings()
        {

            Account account = new SavingsAccount("Customer", "000000", 12000.00);

            account.makeWithdrawal("000000", -900);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WithdrawMoreThanSavingsBalance()
        {

            Account account = new SavingsAccount("Customer", "000000", 12000.00);

            account.makeWithdrawal("000000", 19000);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WithdrawIntoReservedBalance()
        {
            BankPolicy.minimumsavingsBalance = 1000;
            Account account = new SavingsAccount("Customer", "000000", 11000.00);

            account.makeWithdrawal("000000", 11500);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DepositNegAmountToSavings()
        {

            Account account = new SavingsAccount("Customer", "000000", 11000.00);

            account.makeDeposit("000000", -2500);

        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WithdrawNegNumFromCurrent()
        {

            Account account = new CurrentAccount("Customer", "000000", 9000.50);
            account.makeWithdrawal("000000", -900);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WithdrawMoreThanCurrentBalance()
        {

            Account account = new CurrentAccount("Customer", "000000", 110080.00);

            account.makeWithdrawal("000000", 130000);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WithdrawIntoMinimumBalance()
        {

            BankPolicy.minimumcurrentBalance = 5000;
            Account account = new CurrentAccount("Customer", "000000", 11000);

            account.makeWithdrawal("000000", 13500);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DepositNegAmountToCurrent()
        {
            Account account = new CurrentAccount("Customer", "000000", 800);

            account.makeDeposit("000000", -1500);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void LessThanMinimumCurrentBalance()
        {
            BankPolicy.minimumcurrentBalance = 5000;

            Account account = new CurrentAccount("Customer", "000000", 1000);


        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void LessThanMinimumSavingsBalance()
        {
            BankPolicy.minimumsavingsBalance = 1000;

            Account account = new SavingsAccount("Customer", "000000", 500);


        }

        

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInvalidAccNumberOverdraftWithdrawal()
        {
            BankPolicy.minimumcurrentBalance = 1000;
            CurrentAccount account = new CurrentAccount("Customer", "000000", 1200.00);

            double actual = account.overdraftWithdraw("000000", 20000.00);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestCurrentOverdraftWithdrawalWithUnsupportedAmount()
        {

            CurrentAccount account = new CurrentAccount("Customer", "000000", 120000.00);

            account.overdraftWithdraw("000000", 20000.00);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestTransactionWithWrongAccountNumber()
        {

            CurrentAccount account = new CurrentAccount("Customer", "011110", 120000.00);

            account.overdraftWithdraw("011111", 20000.00);

        }

        [TestMethod]
        public void TestRandomStringGenerator()
        {
            string random = AccountNumberGenerator.GenerateUniqueString(10);
            Assert.IsTrue(!"[^a-zA-Z]".Contains(random));
        }

        [TestMethod]
        public void TestCorrectEmailFormat()
        {
            string email = "nennenwodo@gmail.com";
            bool actualResponse = BankDatabaseEngine.checkEmailValidity(email);
            bool expectedResponse = true;
            Assert.AreEqual(actualResponse, expectedResponse);
        }
        [TestMethod]
        public void TestWrongEmailFormat()
        {
            string email = "thisnewemail.com";
            bool actualResponse = BankDatabaseEngine.checkEmailValidity(email);
            bool expectedResponse = false;
            Assert.AreEqual(actualResponse, expectedResponse);
        }

        public void TestCorrectPhoneNumberFormat()
        {
            string phonenumber = "08025657721";
            bool actualResponse = BankDatabaseEngine.checkPhoneNumberValidity(phonenumber);
            bool expectedResponse = true;
             Assert.AreEqual(actualResponse, expectedResponse);
        }
        public void TestWrongPhoneNumberFormat()
        {
            string phonenumber = "+08025657721#";
            bool actualResponse = BankDatabaseEngine.checkPhoneNumberValidity(phonenumber);
            bool expectedResponse = false;
            Assert.AreEqual(actualResponse, expectedResponse);
        }

    

    }
}
