using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;

namespace BankAppConsole
{
    
    /// <summary>
    /// Contains properties of an account.
    /// Contains methods for managing account transactions.
    /// </summary>
    public abstract class Account
    {
        
        /// <summary>
        ///  _AccountBalance member variable
        /// </summary>
        private double _AccountBalance;

        /// <summary>
        /// _AccountName member variable
        /// </summary>
        private readonly string _AccountName;

        /// <summary>
        /// _AccountNumber member variable
        /// </summary>
        private readonly string _AccountNumber;


        /// <summary>
        /// Gets or Sets the account balance
        /// </summary>
        public double AccountBalance
        {
            get { return _AccountBalance; }
            set { _AccountBalance = value; }
        }

        /// <summary>
        /// Gets or Sets the name of an account
        /// </summary>
        public string AccountName
        {
            get { return _AccountName; }
            
        }

        /// <summary>
        /// Gets or Sets the account number
        /// </summary>
        public string AccountNumber
        {
            get { return _AccountNumber; }
            
        }

        /// <summary>
        /// Defines an Account
        /// </summary>
        /// <exception cref="ArgumentException"> Thrown when the initial account balance,
        /// is set to a value less than the minimum account balance allowed.</exception>
        /// <param name="accountBalance">Account Balance of a savings account</param>
        /// <param name="accountName">Name identifying a savings Account</param>
        /// <param name="accountNumber">Unique Number Identifying a savings account</param>
        public Account(string accountName, string accountNumber, double accountBalance)
        {
            _AccountName = accountName;
            _AccountNumber = accountNumber;
            _AccountBalance = accountBalance; 
        }
        
                      
       /// <summary>
       /// Account transaction. Deposits a specified amount into the account 
       /// </summary>
       /// <param name="amount">the amount to deposit</param>
       /// <param name="Accountnumber">Number identifying account.</param>
       /// <returns></returns>
       public abstract double makeDeposit(string Accountnumber, double amount);
       

       /// <summary>
       /// Account transaction. Withdraws a specified amount from the account and saves the changes in the database.
       /// </summary>
       /// <param name="Accountnumber">Number identifying account</param>
       /// <param name="amount">the amount to withdraw</param>
       /// <returns></returns>
       public abstract double makeWithdrawal(string Accountnumber, double amount);
       

     }
}
