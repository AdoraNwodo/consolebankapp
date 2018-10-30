using System;
using System.Collections.Generic;
using BankAppConsole;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BankingAppTest
{
    [TestClass]
    public class UnitTest1
    {
        
        
        [TestMethod]
        public void TestDepositWithSavings()
        {

            SavingsAccount account = new SavingsAccount("Customer", 000000, 12000.00);
            
            double actual = account.makeDeposit(000000, 1000.00);
            double expected = 13000;

            Assert.AreEqual(expected, actual);

        }

        [TestMethod]
        public void TestWithdrawalWithSavings()
        {

            Account<SavingsAccount> account = new Account<SavingsAccount>("Customer", 000000, 12000.00);
            
            double actual = account.makeWithdrawal(000000, 6000);
            double expected = 6000;

            Assert.AreEqual(expected, actual);

        }
        [TestMethod]
        public void TestDepositWithCurrent()
        {

            Account<CurrentAccount> account = new Account<CurrentAccount>("Customer", 000000, 22000.00);
            
            double actual = account.makeDeposit(000000, 10000.00);
            double expected = 32000;

            Assert.AreEqual(expected, actual);

        }

        [TestMethod]
        public void TestWithdrawalWithCurrent()
        {

            Account<CurrentAccount> account = new Account<CurrentAccount>("Customer", 000000, 120000.00);
            
            double actual = account.makeWithdrawal(000000, 60000);
            double expected = 60000;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WithdrawNegNumFromSavings()
        {

            Account<SavingsAccount> account = new Account<SavingsAccount>("Customer", 000000, 12000.00);
            
            account.makeWithdrawal(000000, -900);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WithdrawMoreThanSavingsBalance()
        {

            Account<SavingsAccount> account = new Account<SavingsAccount>("Customer", 000000, 12000.00);
            
            account.makeWithdrawal(000000, 19000);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WithdrawIntoReservedBalance()
        {
            BankPolicy<SavingsAccount>.minimumBalance = 1000;
            Account<SavingsAccount> account = new Account<SavingsAccount>("Customer", 000000, 11000.00);
            
            account.makeWithdrawal(000000, 11500);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DepositNegAmountToSavings()
        {

            Account<SavingsAccount> account = new Account<SavingsAccount>("Customer", 000000, 11000.00);
           
            account.makeDeposit(000000, -2500);

        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WithdrawNegNumFromCurrent()
        {

            Account<CurrentAccount> account = new Account<CurrentAccount>("Customer", 000000, 9000.50);
            account.makeWithdrawal(000000, -900);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WithdrawMoreThanCurrentBalance()
        {

            Account<CurrentAccount> account = new Account<CurrentAccount>("Customer", 000000, 110080.00);
            
            account.makeWithdrawal(000000, 130000);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WithdrawIntoMinimumBalance()
        {

            BankPolicy<CurrentAccount>.minimumBalance = 5000;
            Account<CurrentAccount> account = new Account<CurrentAccount>("Customer", 000000, 11000);
            
            account.makeWithdrawal(000000, 13500);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DepositNegAmountToCurrent()
        {
            Account<CurrentAccount> account = new Account<CurrentAccount>("Customer", 000000, 800);
            
            account.makeDeposit(000000, -1500);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void LessThanMinimumCurrentBalance()
        {
            BankPolicy<CurrentAccount>.minimumBalance = 5000;
            
            Account<CurrentAccount> account = new Account<CurrentAccount>("Customer", 000000, 1000);


        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void LessThanMinimumSavingsBalance()
        {
            BankPolicy<SavingsAccount>.minimumBalance = 1000;
            
            Account<SavingsAccount> account = new Account<SavingsAccount>("Customer", 000000, 500);


        }

        [TestMethod]
        [ExpectedException(typeof(PlatformNotSupportedException))]
        public void OpenWithInvalidAccountType()
        {
            Account<Object> account = new Account<Object>("Customer", 000000, 1000);
        }

        [TestMethod]
        public void TestCurrentOverdraftWithdrawal()
        {

            BankPolicy<CurrentAccount>.minimumBalance = 1000;
            Account<CurrentAccount> account = new Account<CurrentAccount>("Customer", 000000, 1200.00);
            
            double actual = CurrentAccount.overdraftWithdraw(account, 000000, 20000.00);
            double expected = -18800;

            Assert.AreEqual(expected, actual);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestCurrentOverdraftWithdrawalWithUnsupportedAmount()
        {

            Account<CurrentAccount> account = new Account<CurrentAccount>("Customer", 000000, 120000.00);
            
            CurrentAccount.overdraftWithdraw(account, 000000, 20000.00);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestTransactionWithWrongAccountNumber()
        {

            Account<CurrentAccount> account = new Account<CurrentAccount>("Customer", 011110, 120000.00);
            
            CurrentAccount.overdraftWithdraw(account, 011111, 20000.00);

        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestAge()
        {
            Account<CurrentAccount> account = new Account<CurrentAccount>("Customer", 011110, 120000.00);
            Person p = new Person();
            p.age = 17;
            Person.checkAge(p);
        }
    }
}
