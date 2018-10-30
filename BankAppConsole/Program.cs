using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace BankAppConsole
{
    /// <summary>
    /// Main entry point of the application.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Program entry point
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            //set minimum balances and age
            BankPolicy.minimumsavingsBalance = 1000;   
            BankPolicy.minimumcurrentBalance = 10000;
            BankPolicy.MinimumAge = 18;

            BankOperations();

        }

        /// <summary>
        /// Operations this banking application can perform.
        /// Create Account, Deposit, Withdraw, View and OverdraftWithdraw
        /// </summary>
        public static void BankOperations()
        {
            Teller t = new Teller();

            try
            {
                Console.WriteLine("WELCOME TO ILAMOSE BANK");
                Console.WriteLine("");
                Console.WriteLine("Enter the Corresponding Letter for the Action You Wish to Perform");
                Console.WriteLine("");
                Console.WriteLine("Create Account(C), Deposit(D), Withdrawal(W),"
                    +"\n View Account(V), Overdraft Withdrawal(O)(For Current Accounts only)");
                string action = Console.ReadLine();
                switch (action.ToUpper())
                {
                    case "C":
                        // Create account action
                        t.CreateAccount();
                        Console.WriteLine("");
                        Console.WriteLine("Do you want to perform another transaction? \n(y) for yes, anything else for no");
                        string response = Console.ReadLine();
                        if (response == "y")
                            BankOperations(); //new operation
                        else Environment.Exit(0);  //close
                        break;
                    case "D":
                        // Deposit action
                        Console.WriteLine("----------------------------");
                        Console.WriteLine("MAKE DEPOSIT");
                        Console.WriteLine("");
                        Console.WriteLine("Enter Account Number");
                        string creditAccountNumber = Console.ReadLine();
                        if (Teller.AccountExists(creditAccountNumber))
                        {
                            //account exists
                            Console.WriteLine("Enter Deposit Amount");
                            double _creditAmount = double.Parse(Console.ReadLine());
                            t.MakeDeposit(creditAccountNumber, _creditAmount);
                            Console.WriteLine("");
                            Console.WriteLine("Do you want to perform another transaction? \n(y) for yes, anything else for no");
                            string Response = Console.ReadLine();
                            if (Response == "y")
                            {
                                BankOperations(); //new operation
                            }
                            else Environment.Exit(0); //close
                        }
                        else
                        {
                            //account does not exiat
                            Console.WriteLine("Wrong input, account having this id does not exist...");
                            Console.WriteLine("");
                            Console.WriteLine("Do you want to perform another transaction? \n(y) for yes, anything else for no");
                            string Response = Console.ReadLine();
                            if (Response == "y")
                            {
                                BankOperations(); //new operation
                            }
                            else Environment.Exit(0); //close
                        }
                        break;
                    case "W":
                        // Withdrawal action
                        Console.WriteLine("----------------------------");
                        Console.WriteLine("MAKE WITHDRAWAL");
                        Console.WriteLine("");
                        Console.WriteLine("Enter Account Number");
                        string debitAccountNumber = Console.ReadLine();
                        if (Teller.AccountExists(debitAccountNumber))
                        {
                            //account exists
                            Console.WriteLine("Enter Withdrawal Amount");
                            double _debitAmount = double.Parse(Console.ReadLine());
                            t.MakeDeposit(debitAccountNumber, _debitAmount);
                            Console.WriteLine("");
                            Console.WriteLine("Do you want to perform another transaction? \n(y) for yes, anything else for no");
                            string Response = Console.ReadLine();
                            if (Response == "y")
                            {
                                BankOperations();  //new operation
                            }
                            else Environment.Exit(0); //close
                        }
                        else
                        {
                            //account does not exist
                            Console.WriteLine("Wrong input, account having this id does not exist...");
                            Console.WriteLine("");
                            Console.WriteLine("Do you want to perform another transaction? \n(y) for yes, anything else for no");
                            string Response = Console.ReadLine();
                            if (Response == "y")
                            {
                                BankOperations(); //new operation
                            }
                            else Environment.Exit(0);  //close
                        }
                        break;
                    case "V":
                        // view account
                        Console.WriteLine("----------------------------");
                        Console.WriteLine("VIEW ACCOUNT DETAILS...");
                        Console.WriteLine("");
                        Console.WriteLine("Enter Account Number");
                        string accountnumber = Console.ReadLine();
                        if (Teller.AccountExists(accountnumber))
                        {
                            //account exists
                            t.ViewAccountInfo(accountnumber);
                        }
                        else Console.WriteLine("You didnt type a valid account id number");  //account does not exist
                        Console.WriteLine("Do you want to perform another transaction? \n(y) for yes, anything else for no");
                        string response_ = Console.ReadLine();
                        if (response_ == "y")
                        {
                            BankOperations();  //new operation
                        }
                        else Environment.Exit(0);  //close
                        break;
                    case "O":
                        // overdraft withdrawal
                        Console.WriteLine("----------------------------");
                        Console.WriteLine("MAKE OVERDRAFT WITHDRAWAL");
                        Console.WriteLine("");
                        Console.WriteLine("Enter Account Number");
                        string overdraftAccountNumber = Console.ReadLine();
                        if (Teller.GetAccountType(overdraftAccountNumber).Equals("current".ToLower()))
                        { //current account exists
                            Console.WriteLine("Enter Withdrawal Amount");
                            double debitAmount = double.Parse(Console.ReadLine());
                            t.MakeOverdraftWithdrawal(overdraftAccountNumber, debitAmount);
                            Console.WriteLine("");
                            Console.WriteLine("Do you want to perform another transaction? \n(y) for yes, anything else for no");
                            string Response = Console.ReadLine();
                            if (Response == "y")
                            {
                                BankOperations();  //new operation
                            }
                            else Environment.Exit(0);  //close
                        }
                        else
                        {
                            //curren account does not exist
                            Console.WriteLine("Current account having this id does not exist...");
                            Console.WriteLine("");
                            Console.WriteLine("Do you want to perform another transaction? \n(y) for yes, anything else for no");
                            string Response = Console.ReadLine();
                            if (Response == "y")
                            {
                                BankOperations();  //new operation
                            }
                            else Environment.Exit(0);  //close
                        }
                        break;

                    default:
                        Console.WriteLine("Your Choice wasn't part of the options");
                        break;
                }
            }
            catch (FormatException) { Console.WriteLine("Input was of the wrong format..."); }
            catch (ArgumentException ) { Console.WriteLine("Cannot perform operation, please check that you typed correctly...");}
            catch (InvalidDataException) { Console.WriteLine("Please check input..."); }
            catch (InvalidOperationException) { Console.WriteLine("Not eligible to perform this particular operation..."); }

            finally
            {
                Console.WriteLine("Do you want to perform another transaction? \n(y) for yes, anything else for no");
                string Resp = Console.ReadLine();
                if (Resp == "y")
                {
                    //new operation
                    BankOperations();
                }
                else Environment.Exit(0);  //close
            }
            Console.Read();
        }
    }
}