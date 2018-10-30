using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;


namespace BankAppConsole
{
    /// <summary>
    /// Contains methods defining transactions a teller can perform on an existing account.
    /// Teller can also open new account for a customer.
    /// </summary>
    public class Teller : BankDatabaseEngine
    {
                
        /// <summary>
        /// Creates a new account for a customer and saves the account details to the database.
        /// Requires: The Banks Policy conditions for minimum age to be eligible to open current and savings accounts are set.
        /// Requires: The Banks Policy conditions for minimum amount to be eligible to open current and savings accounts are set.
        /// </summary>
        /// <exception cref="InvalidDataException">Thrown when user enters wrong account type or gender</exception>
        /// <exception cref="InvalidOperationException">Thrown when the customer is not eligible to open any account</exception>
        /// <exception cref="FormatException">Thrown when user input is of the wrong format</exception>
        public virtual void CreateAccount()
        {
            Contract.Requires(BankPolicy.MinimumAge > 0);
            Contract.Requires(BankPolicy.minimumcurrentBalance > 0 || BankPolicy.minimumsavingsBalance >= 0);
            Console.WriteLine("----------------------------");
            Console.WriteLine("CREATE ACCOUNT");
            Console.WriteLine("Enter customer name");
            Name = Console.ReadLine();
            Console.WriteLine("Enter Customer Gender");
            Console.WriteLine("Male(M), Female(F)");
            string customerGender = (Console.ReadLine());
            switch (customerGender.ToUpper())
            {
                case "M":
                    customerGender = "Male";
                    break;
                case "F":
                    customerGender = "Female";
                    break;
                default:
                    throw new InvalidDataException("Wrong Gender!");
            }
            Console.WriteLine("Enter Birthday (mm/dd/yyyy)");
            Birthday = DateTime.Parse(Console.ReadLine());
            
            if (DateTime.Today.Year - Birthday.Year < BankPolicy.MinimumAge)
                throw new InvalidOperationException("Please you are too young to open an account");
              
            Console.WriteLine("Enter Customer Phone Number");
            phonenumber = Console.ReadLine();

            if (!checkPhoneNumberValidity(phonenumber).Equals(true))
                throw new FormatException("Please check that the phone number format is correct");
            
            Console.WriteLine("Enter Customer Email Address");
            email_address = Console.ReadLine();

            if (!checkEmailValidity(email_address).Equals(true))
                throw new FormatException("Not an email address, please check input");
            
            Console.WriteLine("Enter Customer House Address");
            house_address = Console.ReadLine();
            Console.WriteLine("");

            Console.WriteLine("Enter Preferred Account Name");
            string accountName = Console.ReadLine();
            double accountBalance;
            
            Console.WriteLine("What Type of Account?");
            Console.WriteLine("Savings(S), Current(C)");
            string accountType = Console.ReadLine();
            switch (accountType.ToUpper())
            {
                case "S":
                    accountType = "Savings";
                    Console.WriteLine("Enter Initial Deposit Amount");
                    accountBalance = double.Parse(Console.ReadLine());
                    if (accountBalance < BankPolicy.minimumsavingsBalance)
                        throw new InvalidOperationException("Please check the banks policies for minimum balance for savings accounts");
                    break;
                case "C":
                    accountType = "Current";
                    Console.WriteLine("Enter Initial Deposit Amount");
                    accountBalance = double.Parse(Console.ReadLine());
                    if (accountBalance < BankPolicy.minimumcurrentBalance)
                        throw new InvalidOperationException("Please check the banks policies for minimum balance for savings accounts");
                    break;
                default:
                    throw new InvalidDataException("Wrong Account type");
            }
                       
            string accountNumber = AccountNumberGenerator.GenerateUniqueString(10);    //crypto random number generator for account number
           
            
            
            
            // Save the account to the database
            SaveAccount(accountNumber, accountType, accountName, accountBalance, Name, customerGender, phonenumber, Birthday, email_address, house_address);
            Console.WriteLine("Congrats! Your account number is "+accountNumber);
       }  //end CreateAccount method

        /// <summary>
        /// Allows the Teller credit a certain account by entering the account number and the amount to be deposited.
        /// Requires: The account number is currently in the banks database.
        /// Requires: The amount to be deposited is not negative.
        /// </summary>
        /// <param name="accountNumber">The account number identifying the account to be deposited to.</param>
        /// <param name="creditAmount">Amount to be deposited.</param>
        /// <exception cref="InvalidDataException">Thrown when the account number does not exist in the database</exception>
        /// <exception cref="ArgumentException">Thrown when the Amount to be deposited is negative.</exception>
        public virtual void MakeDeposit(string accountNumber, double creditAmount)
        {

            Contract.Requires<InvalidDataException>(AccountExists(accountNumber), "Please check input. Account Number does not exist");
            Contract.Requires<ArgumentException>(creditAmount > 0, "Cannot deposit a negative balance");
            Console.WriteLine("");
            Console.WriteLine("ACCOUNT INFO");
            Console.WriteLine("");
            string accountName = GetAccountName(accountNumber);
            Console.WriteLine("Account Name: " + accountName );
            string accountType = GetAccountType(accountNumber);
            Console.WriteLine("Account Type: " + accountType);
            Console.WriteLine("Account Number: " + accountNumber);
            
            double currentBalance = GetAccountBalance(accountNumber);
            Console.WriteLine("Previous Account Balance: " + getBalance(currentBalance));
            Console.WriteLine("");
            switch (accountType.ToUpper())
            {
                case "SAVINGS":               //credits the savings account
                    Account savingsAcc = new SavingsAccount(accountName, accountNumber, currentBalance);
                    savingsAcc.makeDeposit(accountNumber, creditAmount);
                    break;
                case "CURRENT":              //credits the current account
                    Account currentAcc = new CurrentAccount(accountName, accountNumber, currentBalance);
                    currentAcc.makeDeposit(accountNumber, creditAmount);
                    break;
            }
            Console.WriteLine("New Account Balance: " + getBalance(GetAccountBalance(accountNumber)));
        }  //end MakeDeposit method

        /// <summary>
        /// Allows the Teller debit a certain account by entering the account number and the amount to be withdrawn.
        /// Requires: The account number is currently in the banks database.
        /// Requires: The amount to be withdrawn is not negative.
        /// Requires: The amount to be withdrawn is not greater than the present account balance.
        /// </summary>
        /// <param name="accountNumber">The account number identifying the account to withdraw from.</param>
        /// <param name="debitAmount">Amount to be withdrawn.</param>
        /// <exception cref="InvalidDataException">Thrown when the account number does not exist in the database</exception>
        /// <exception cref="ArgumentException">Thrown when the Amount to be withdrawn is negative,
        /// Thrown when the debit amount is greater than the present account balance.
        /// </exception>
        public virtual void MakeWithdrawal(string accountNumber, double debitAmount)
        {
            Contract.Requires<InvalidDataException>(AccountExists(accountNumber),"Please check input. Account Number does not exist");
            Contract.Requires<ArgumentException>(debitAmount > 0, "Cannot withdraw a negative balance");
            Contract.Requires<ArgumentException>(GetAccountBalance(accountNumber) >= debitAmount,
                "Cannot withdraw more than what you have, try using the overdraft");
           
            Console.WriteLine("");
            Console.WriteLine("ACCOUNT INFO");
            Console.WriteLine("");
            string accountName = GetAccountName(accountNumber);
            Console.WriteLine("Account Name: " + accountName);
            string accountType = GetAccountType(accountNumber);
            Console.WriteLine("Account Type: " + accountType);
            Console.WriteLine("Account Number: " + accountNumber);
            double currentBalance = GetAccountBalance(accountNumber);
            Console.WriteLine("Previous Account Balance: " + getBalance(currentBalance));
            Console.WriteLine("");
            

            
            switch (accountType.ToUpper())
            {
                case "SAVINGS":         //debits the savings account
                    Account savingsAcc = new SavingsAccount(accountName, accountNumber, currentBalance);
                    savingsAcc.makeWithdrawal(accountNumber, debitAmount);
                    break;
                case "CURRENT":        //debits the savings account
                    Account currentAcc = new CurrentAccount(accountName, accountNumber, currentBalance);
                    currentAcc.makeWithdrawal(accountNumber, debitAmount);
                    break;
            }
            Console.WriteLine("New Account Balance: " +getBalance(GetAccountBalance(accountNumber)));
        }  //end MakeWithdrawal method

        /// <summary>
        /// Allows the Teller debit a certain account by entering the account number and the amount to be withdrawn.
        /// Requires: The account number is currently in the banks database.
        /// Requires: The amount to be withdrawn is not negative.
        /// Requires: The account is a current account.
        /// Requires: The amount to be withdrawn is greater than the present amount.
        /// Requires: The account balance value is positive.
        /// </summary>
        /// <param name="accountNumber">The account number identifying the account to withdraw from.</param>
        /// <param name="debitAmount">Amount to be withdrawn</param>
        /// <exception cref="InvalidDataException">Thrown when the account number does not exist in the database</exception>
        /// <exception cref="InvalidOperationException">Thrown when the account is not a current account.</exception>
        /// <exception cref="ArgumentException">Thrown when the Amount to be withdrawn is negative,
        /// Thrown when the debit amount is less than the present account balance,
        /// Thrown when the account balance is less than zero.
        /// </exception>
        public virtual void MakeOverdraftWithdrawal(string accountNumber, double debitAmount)
        {
            Contract.Requires<ArgumentException>(GetAccountBalance(accountNumber) > 0, "You already owe the bank");
            Contract.Requires<InvalidDataException>(AccountExists(accountNumber), "Please check input. Account Number does not exist");
            Contract.Requires<InvalidOperationException>(GetAccountType(accountNumber).ToLower().Equals("current"),
                "Transaction valid for current accounts only.");
            Contract.Requires<ArgumentException>(GetAccountBalance(accountNumber) < debitAmount,
                "Can withdraw normally, no need for overdraft");
            Contract.Requires<ArgumentException>(debitAmount > 0, "Cannot withdraw a negative balance");
            Console.WriteLine("");
            Console.WriteLine("ACCOUNT INFO");
            Console.WriteLine("");
            string accountName = GetAccountName(accountNumber);
            Console.WriteLine("Account Name: " + accountName);
            string accountType = GetAccountType(accountNumber);
            Console.WriteLine("Account Type: " + accountType);
            Console.WriteLine("Account Number: " + accountNumber);
            Console.WriteLine("");
            
            double currentBalance = GetAccountBalance(accountNumber);
            Console.WriteLine("Previous Account Balance: " + getBalance(currentBalance));
            CurrentAccount currAcc = new CurrentAccount(accountName,accountNumber,currentBalance);
            currAcc.overdraftWithdraw(accountNumber,debitAmount);
            Console.WriteLine("New Account Balance: " + getBalance(GetAccountBalance(accountNumber)));

            
        }  //end MakeOverdraftWithdrawal method
        
        /// <summary>
        /// Displays the Account name, Account type, Account number and account balance for a particular account.
        /// Requires: The account number exists in the banks database.
        /// </summary>
        /// <param name="accountNumber">Unique number identifying the account.</param>
        /// <exception cref="InvalidDataException">Thrown when the account number does not exist in the banks database</exception>
        public virtual void ViewAccountInfo(string accountNumber)
        {
            Contract.Requires<InvalidDataException>(AccountExists(accountNumber), "Please check input. Account Number does not exist");

            Console.WriteLine("");
            Console.WriteLine("ACCOUNT INFO");
            Console.WriteLine("");
            Console.WriteLine("Account Name: "+ GetAccountName(accountNumber));
            Console.WriteLine("Account Type: "+ GetAccountType(accountNumber));
            Console.WriteLine("Account Number: "+ accountNumber);
            Console.WriteLine("Account Balance: "+getBalance(GetAccountBalance(accountNumber)));
        }  //end ViewAccountInfo method

                
        /// <summary>
        /// Computes the account balance in currency format.
        /// Current accounts can have negative balances due to overdraft.
        /// ($ ###,###) indicates the amount a customer owes a bank if (s)he withdrew using overdraft.
        /// </summary>
        /// <param name="AccountBalance">Account balance</param>
        /// <returns>Account balance of a</returns>
        public virtual string getBalance(double AccountBalance)
        {
            return string.Format("{0:C}", AccountBalance);  //currency format
        } //end getBalance method

        
    }
}
