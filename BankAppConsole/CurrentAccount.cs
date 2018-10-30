using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace BankAppConsole
{
    /// <summary>
    /// Current account.
    /// Represents operations particular to only current accounts.
    /// </summary>
    public class CurrentAccount : Account
    {

        /// <summary>
        /// Defines a Current Account
        /// Requires: The account balance never exceeds its minimum limit
        /// </summary>
        /// <exception cref="ArgumentException"> Thrown when the initial account balance,
        /// is set to a value less than the minimum account balance allowed.</exception>
        /// <param name="accountBalance">Account Balance of a current account</param>
        /// <param name="accountName">Name identifying a current Account</param>
        /// <param name="accountNumber">Unique Number Identifying a current account</param>
        public CurrentAccount(string accountName, string accountNumber, double accountBalance) :
            base(accountName, accountNumber, accountBalance)
        { Contract.Requires<ArgumentException>(accountBalance >= BankPolicy.minimumcurrentBalance); }

        /// <summary>
        /// Current Account transaction. Deposits a specified amount into the savings account and saves the changes in the database.
        /// Requires: The amount to be deposited should not be negative
        /// Requires: The account number is valid
        /// </summary>
        /// <param name="amount">the amount to deposit</param>
        /// <param name="Accountnumber">Number identifying account.</param>
        /// <exception cref="ArgumentException"> Thrown when a negative amount deposit is attempted or,
        /// when the input account number is not same as 
        /// the account number registered for the account.</exception>
        /// <returns>The new account balance</returns>
        public override double makeDeposit(string Accountnumber, double amount)
        {
            Contract.Requires<ArgumentException>(amount > 0, "Invalid amount. Please check input");

            Contract.Requires<ArgumentException>(AccountNumber.Equals(Accountnumber), "Invalid account number, check again");
            Contract.Ensures(Contract.OldValue<double>(AccountBalance) < AccountBalance);
            Contract.Ensures((Contract.OldValue<double>(AccountBalance) + amount).Equals(AccountBalance));
            AccountBalance = AccountBalance + amount;

            // Credit the account in the database
            BankDatabaseEngine.CreditAccount(Accountnumber, amount);

            return AccountBalance;

        }//end makeDeposit method

        /// <summary>
        /// Current Account transaction. Withdraws a specified amount from the current account and saves the changes in the database.
        /// Requires: The amount to be withdrawn should not be negative
        /// Requires: The amount to be withdrawn should be less than or equal to the account balance
        /// Requires: The account balance should be more than the minimum balance allowed for the bank
        /// Requires: The account number is valid
        /// </summary>
        /// <param name="Accountnumber">Number identifying account</param>
        /// <param name="amount">the amount to withdraw</param>
        /// <exception cref="ArgumentException">Thrown when the account balance is not sufficient for the transaction or,
        /// when the amount to withdraw is negative or, 
        /// when the input account number is not same as 
        /// the account number registered for the account.</exception>
        /// <returns>The new account balance</returns>
        public override double makeWithdrawal(string Accountnumber, double amount)
        {
            Contract.Requires<ArgumentException>(amount > 0, "Invalid amount. Please check input");
            Contract.Requires<ArgumentException>(amount <= (AccountBalance - BankPolicy.minimumcurrentBalance), "Cannot withdraw more than the available balance");
            Contract.Requires<ArgumentException>(AccountBalance >= BankPolicy.minimumcurrentBalance, "Account balance is Insufficient for this transaction");
            Contract.Requires<ArgumentException>(AccountNumber.Equals(Accountnumber), "Invalid account number, check again");
            Contract.Ensures(AccountBalance >= BankPolicy.minimumcurrentBalance);
            Contract.Ensures(Contract.OldValue<double>(AccountBalance) > AccountBalance);
            Contract.Ensures((Contract.OldValue<double>(AccountBalance) - amount).Equals(AccountBalance));

            AccountBalance = AccountBalance - amount;

            // Debit the account in the database
            BankDatabaseEngine.DebitAccount(Accountnumber, amount);

            return AccountBalance;
        }//end makeWithdrawal method

        /// <summary>
        /// Account transaction for Current Account. 
        /// Withdraws a specified amount from the account and saves the changes in the database.
        /// Amount to be withdrawn will be more than the account balance.(Overdraft).
        /// Requires: The amount to be withdrawn is more than the current account balance. 
        /// Requires: The account number is valid.
        /// Requires: The account balance value is positive.
        /// </summary>
        /// <param name="Accountnumber">Number identifying account</param>
        /// <param name="amount">the amount to withdraw</param>
        /// <exception cref="ArgumentException">Thrown when the amount to withdraw can be withdrawn without overdraft or,
        /// when the input account number is not same as 
        /// the account number registered for the account.</exception>
        /// <returns>The new account balance</returns>
        public double overdraftWithdraw(string Accountnumber, double amount)
        {
            //overdraft allows withdrawal to exceed the account balance
            Contract.Requires<ArgumentException>(this.AccountBalance > 0, "You already owe the bank");
            Contract.Requires<ArgumentException>(amount > this.AccountBalance, "Cannot use overdraft. Withdraw the normal way");
            Contract.Requires<ArgumentException>(this.AccountNumber.Equals(Accountnumber), "Invalid account number, check again");
            Contract.Ensures(Contract.OldValue<double>(this.AccountBalance) > this.AccountBalance);
            Contract.Ensures((Contract.OldValue<double>(this.AccountBalance) - amount).Equals(this.AccountBalance));
            Contract.Ensures(this.AccountBalance < 0);

            this.AccountBalance = this.AccountBalance - amount;    //negative balance due to overdraft
            BankDatabaseEngine.DebitAccount(Accountnumber, amount);  //debit the account in the database
            return this.AccountBalance;
        } //end overDraft method

        
    }
}