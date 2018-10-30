using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;
using System.Data.OleDb;
using System.Configuration;
using System.Data;
using System.Text.RegularExpressions;

namespace BankAppConsole
{
    /// <summary>
    /// Controls the banks database.
    /// Can add a new account to the database and retrieve information from the database.
    /// </summary>
    public class BankDatabaseEngine: Person
    {
        
        /// <summary>
        /// Updates database by adding new account.
        /// Requires: The email address is of a valid format.
        /// </summary>
        /// <param name="accountNumber">Customers Account Number</param>
        /// <param name="accountType">Type of account customer opened</param>
        /// <param name="accountName">Name of account</param>
        /// <param name="accountBalance">Start amount in the new account</param>
        /// <param name="customerName">Name of customer</param>
        /// <param name="gender">Gender of customer</param>
        /// <param name="phoneNumber">Phone number of customer</param>
        /// <param name="dob">Birthday of customer</param>
        /// <param name="dob">Customers birthday</param>
        /// <param name="emailAddress">Email address of customer</param>
        /// <param name="houseAddress">House address of customer</param>
        /// <exception cref="FormatException">Thrown when users phone number or email are of the wrong format</exception>
        public static void SaveAccount(string accountNumber, string accountType, string accountName, 
            double accountBalance, string customerName, string gender, string phoneNumber, DateTime dob, string emailAddress, string houseAddress)
        {
            Contract.Requires<FormatException>(checkEmailValidity(emailAddress).Equals(true), "Not an email address, please check input");
            Contract.Requires<FormatException>(checkPhoneNumberValidity(phoneNumber).Equals(true), "Please check that the phone number is correct");
            
            string connString = Properties.Settings.Default.connStrBankAppConsole;    //database connection string.
            OleDbConnection conn = new OleDbConnection(connString);
            string query = @"INSERT INTO BankAccounts VALUES(@AccountNumber,@AccountType,@AccountName,@AccountBalance,@CustomerName,@Gender,@PhoneNumber,@Birthday,@EmailAddress, @HouseAddress)";
            conn.Open();
            OleDbCommand cmd = new OleDbCommand(query, conn);
            cmd.Parameters.AddWithValue("@AccountNumber", accountNumber);
            cmd.Parameters.AddWithValue("@AccountType", accountType);
            cmd.Parameters.AddWithValue("@AccountName", accountName);
            cmd.Parameters.AddWithValue("@AccountBalance", accountBalance);
            cmd.Parameters.AddWithValue("@CustomerName", customerName);
            cmd.Parameters.AddWithValue("@Gender", gender);
            cmd.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
            cmd.Parameters.AddWithValue("@Birthday", dob);
            cmd.Parameters.AddWithValue("@EmailAddress", emailAddress);
            cmd.Parameters.AddWithValue("@HouseAddress", houseAddress);
            // Execute the command
            int i = cmd.ExecuteNonQuery();
            if (i > 0)
            {
                Console.WriteLine("The Account was Created Successfully");
            }
            else
            {
                // The save action failed
                Console.WriteLine("Account Creation Failed");
            }
            conn.Close();
        }// end SaveAccount method


        /// <summary>
        /// Finds the account balance corresponding to the account number and returns.
        /// Requires: The account exists in the database.
        /// </summary>
        /// <param name="accountNumber">The account number to search</param>
        /// <exception cref="ArgumentException">Thrown when the account number does not exist in the database.</exception>
        /// <returns>The Account Balance</returns>
        public static double GetAccountBalance(string accountNumber)
        {
            Contract.Requires<ArgumentException>(AccountExists(accountNumber), "Account does not exist.");
            string connString = Properties.Settings.Default.connStrBankAppConsole;
            OleDbConnection conn = new OleDbConnection(connString);
            conn.Open(); // Open the connection
            DataTable dt = new DataTable();
            dt.Clear(); // Clear the data table
            string query = @"SELECT AccountBalance FROM BankAccounts WHERE AccountNumber = @AccountNumber";
            OleDbDataAdapter da = new OleDbDataAdapter(query, conn);
            da.SelectCommand.Parameters.AddWithValue("@AccountNumber", accountNumber);
            da.Fill(dt);
            double accountBalance = 0;
            if (dt.Rows.Count != 0)
            {
                accountBalance = double.Parse(dt.Rows[0]["AccountBalance"].ToString());
            }
            da.Dispose();
            dt.Clear();
            conn.Close();
            return accountBalance;
        }

        /// <summary>
        /// Finds the account type corresponding to the specified account number.
        /// Account Type is current or savings.
        /// Requires: The account exists in the database.
        /// </summary>
        /// <param name="accountNumber">The account number to search</param>
        /// <exception cref="ArgumentException">Thrown when the account number does not exist in the database.</exception>
        /// <returns>The account type</returns>
        public static string GetAccountType(string accountNumber)
        {
            Contract.Requires<ArgumentException>(AccountExists(accountNumber), "Account does not exist.");
            string connString = Properties.Settings.Default.connStrBankAppConsole;
            OleDbConnection conn = new OleDbConnection(connString);
            conn.Open(); // Open the connection
            DataTable dt = new DataTable();
            dt.Clear(); // Clear the data table
            string query = @"SELECT AccountType FROM BankAccounts WHERE AccountNumber = @AccountNumber";
            OleDbDataAdapter da = new OleDbDataAdapter(query, conn);
            da.SelectCommand.Parameters.AddWithValue("@AccountNumber", accountNumber);
            da.Fill(dt);
            string accountType = "";
            if (dt.Rows.Count != 0)
            {
                accountType = dt.Rows[0]["AccountType"].ToString();
            }
            da.Dispose();
            dt.Clear();
            conn.Close();
            return accountType;
        }

        /// <summary>
        /// Finds the account name corresponding to the specified account number.
        /// Requires: The account exists in the database.
        /// </summary>
        /// <param name="accountNumber">The account number to search</param>
        /// <exception cref="ArgumentException">Thrown when the account number does not exist in the database.</exception>
        /// <returns>The account name</returns>
        public static string GetAccountName(string accountNumber)
        {
            Contract.Requires<ArgumentException>(AccountExists(accountNumber), "Account does not exist.");
            string connString = Properties.Settings.Default.connStrBankAppConsole;
            OleDbConnection conn = new OleDbConnection(connString);
            conn.Open(); // Open the connection
            DataTable dt = new DataTable();
            dt.Clear(); // Clear the data table
            string query = @"SELECT AccountName FROM BankAccounts WHERE AccountNumber = @AccountNumber";
            OleDbDataAdapter da = new OleDbDataAdapter(query, conn);
            da.SelectCommand.Parameters.AddWithValue("@AccountNumber", accountNumber);
            da.Fill(dt);
            string accountName = "";
            if (dt.Rows.Count != 0)
            {
                accountName = dt.Rows[0]["AccountName"].ToString();
            }
            da.Dispose();
            dt.Clear();
            conn.Close();
            return accountName;
        }

        /// <summary>
        /// Updates the account in the database,
        /// by crediting the account corresponding to the specific account number in the database with the amount specified.
        /// Requires: The account exists in the database.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when the account number does not exist in the database.</exception>
        /// <param name="accountNumber">account number corresponding to the account to be credited.</param>
        /// <param name="creditAmount">amount to deposit.</param>
        public static void CreditAccount(string accountNumber, double creditAmount)
        {
            Contract.Requires<ArgumentException>(AccountExists(accountNumber), "Account does not exist.");
            // Get the current account balance
            double currentAccountBalance = GetAccountBalance(accountNumber);
            
            // Compute the new account balance
            double newAccountBalance = currentAccountBalance + creditAmount;

            string connString = Properties.Settings.Default.connStrBankAppConsole;
            OleDbConnection conn = new OleDbConnection(connString);
            string query = @"UPDATE BankAccounts SET AccountBalance = ? WHERE AccountNumber = ?";
            conn.Open();
            OleDbCommand cmd = new OleDbCommand(query, conn);
            cmd.Parameters.AddWithValue("?", newAccountBalance);
            cmd.Parameters.AddWithValue("?", accountNumber);
            // Execute the command
            int i = cmd.ExecuteNonQuery();
            if (i > 0)
            {
                Console.WriteLine("The Account was Credited Successfully");
            }
            else
            {
                // The update action failed
                Console.WriteLine("Credit Action Failed");
            }
            conn.Close();
        }

       
        /// <summary>
        /// Updates the account in the database,
        /// by debitting the account corresponding to the specific account number in the database with the amount specified.
        /// Requires: The account exists in the database.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when the account number does not exist in the database.</exception>
        /// <param name="accountNumber">account number corresponding to the account to be debitted.</param>
        /// <param name="debitAmount">amount to withdraw</param>
        public static void DebitAccount(string accountNumber, double debitAmount)
        {
            Contract.Requires<ArgumentException>(AccountExists(accountNumber), "Account does not exist.");
            // Get the current account balance
            double currentAccountBalance = GetAccountBalance(accountNumber);

              // Compute the new account balance
                double newAccountBalance = currentAccountBalance - debitAmount;

                string connString = Properties.Settings.Default.connStrBankAppConsole;
                OleDbConnection conn = new OleDbConnection(connString);
                string query = @"UPDATE BankAccounts SET AccountBalance = ? WHERE AccountNumber = ?";
                conn.Open();
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("?", newAccountBalance);
                cmd.Parameters.AddWithValue("?", accountNumber);
                // Execute the command
                int i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    Console.WriteLine("The Account was Debited Successfully");
                }
                else
                {
                    // The update action failed
                    Console.WriteLine("Debit Action Failed");
                }
                conn.Close();
                       
        }

        /// <summary>
        /// Gets information about the customer owning the account with the specified account number.
        /// Requires: The account exists in the database.
        /// </summary>
        /// <param name="accountNumber">account number corresponding to the account of the customer.</param>
        /// <exception cref="ArgumentException">Thrown when the account number does not exist in the database.</exception>
        /// <returns>The Persons information.</returns>
        public static Person GetCustomerInfo(string accountNumber)
        {
            Contract.Requires<ArgumentException>(AccountExists(accountNumber), "Account does not exist.");
            string connString = Properties.Settings.Default.connStrBankAppConsole;
            OleDbConnection conn = new OleDbConnection(connString);
            conn.Open(); // Open the connection
            DataTable dt = new DataTable();
            dt.Clear(); // Clear the data table
            string query = @"SELECT CustomerName, Gender, PhoneNumber, Age, EmailAddress, HouseAddress FROM BankAccounts WHERE AccountNumber = @AccountNumber";
            OleDbDataAdapter da = new OleDbDataAdapter(query, conn);
            da.SelectCommand.Parameters.AddWithValue("@AccountNumber", accountNumber);
            da.Fill(dt);
            Person customer = null;
            if (dt.Rows.Count != 0)
            {
                customer = new Person();
                customer.Name = dt.Rows[0]["CustomerName"].ToString();
                switch (dt.Rows[0]["Gender"].ToString().ToUpper())
                {
                    case "MALE":
                        customer.gender = Gender.Male;
                        break;
                    case "FEMALE":
                        customer.gender = Gender.Female;
                        break;
                }
                customer.phonenumber = dt.Rows[0]["PhoneNumber"].ToString();
                customer.Birthday = DateTime.Parse(dt.Rows[0]["Birthday"].ToString());
                customer.email_address = dt.Rows[0]["EmailAddress"].ToString();
                customer.house_address = dt.Rows[0]["HouseAddress"].ToString();
            }
            da.Dispose();
            dt.Clear();
            conn.Close();
            return customer;
        }

        /// <summary>
        /// Uses the account number to check if a given account exists.
        /// An account exists when the account number is found in the database.
        /// </summary>
        /// <param name="accountNumber">the account number to search</param>
        /// <returns>"true" if account exists, false if not.</returns>
        public static bool AccountExists(string accountNumber)
        {
            bool response = false;
            string connString = Properties.Settings.Default.connStrBankAppConsole;
            OleDbConnection conn = new OleDbConnection(connString);
            conn.Open(); // Open the connection
            DataTable dt = new DataTable();
            dt.Clear(); // Clear the data table
            // Select a column from the BankAccounts table
            string query = @"SELECT AccountType FROM BankAccounts WHERE AccountNumber = @AccountNumber";
            OleDbDataAdapter da = new OleDbDataAdapter(query, conn);
            da.SelectCommand.Parameters.AddWithValue("@AccountNumber", accountNumber);
            da.Fill(dt);
            if (dt.Rows.Count != 0)
            {
                // The account exists
                response = true;
            }
            else
            {
                // The account does not exist
                response = false;
            }
            da.Dispose();
            dt.Clear();
            conn.Close();
            return response;
        }

        /// <summary>
        /// Uses Regular Expressions to check if email address is of the correct format
        /// </summary>
        /// <param name="email">email to check</param>
        /// <returns></returns>
        private static bool IsEmailValid(string email)
        {
            return Regex.IsMatch(email,
                @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }

        /// <summary>
        /// Uses Regular Expressions to check if email address is of the correct format
        /// </summary>
        /// <param name="email">email to check</param>
        /// <returns></returns>
         public static bool checkEmailValidity(string email)
        {
            return IsEmailValid(email);
        }

        /// <summary>
        /// Uses Regular Expressions to check if phone number is of the correct format
        /// </summary>
        /// <param name="phonenumber">number to check</param>
        /// <returns></returns>
        private static bool IsPhoneNumberValid(string phonenumber)
        {
            return Regex.IsMatch(phonenumber,@"[0-9]");
        }

        /// <summary>
        /// Uses Regular Expressions to check if phone number is of the correct format
        /// </summary>
        /// <param name="phonenumber">number to check</param>
        /// <returns></returns>
        public static bool checkPhoneNumberValidity(string phonenumber)
        {
            return IsPhoneNumberValid(phonenumber);
        }

    }
}
