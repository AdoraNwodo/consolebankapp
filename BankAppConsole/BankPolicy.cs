using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace BankAppConsole
{
    /// <summary>
    /// Defines the Bank Policies for some operations on savings and current accounts.
    /// </summary>
    public class BankPolicy
    {
        /// <summary>
        /// Variable defining the minimum savings balance
        /// </summary>
        private static double MinimumSavingsBalance;

        /// <summary>
        /// Variable defining the minimum current balance
        /// </summary>
        private static double MinimumCurrentBalance;

        
        /// <summary>
        /// Defines the minimum age to open an account
        /// </summary>
        private static int minimumAge;

        /// <summary>
        /// gets or sets the minimum amount that can be in a savings account at any given time
        /// </summary>
        public static double minimumsavingsBalance
        {
            get { return MinimumSavingsBalance; }
            set { MinimumSavingsBalance = value; }
        }

        /// <summary>
        /// gets or sets the minimum amount that can be in a current account at any given time
        /// </summary>
        public static double minimumcurrentBalance
        {
            get { return MinimumCurrentBalance; }
            set { MinimumCurrentBalance = value; }
        }


        /// <summary>
        /// gets or sets the minimum age required to open an account type
        /// </summary>
        public static int MinimumAge 
        {
            get { return minimumAge; }
            set { minimumAge = value; }
        }
    }
}
