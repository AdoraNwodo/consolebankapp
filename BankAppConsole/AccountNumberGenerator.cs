using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;



namespace BankAppConsole
{
    /// <summary>
    /// Contains a method used to generate account numbers for customers
    /// </summary>
    public class AccountNumberGenerator
    {
        
        /// <summary>
        /// Uses the crypto random number generator to generate unique strings of a specified length
        /// </summary>
        /// <param name="length">The length of the string to be generated</param>
        /// <returns>The generated string</returns>
        internal static string GenerateUniqueString(int length)
        {
            string a = "1234567890";     //string should be numeric
            char[] chars = new char[a.Length];
            chars = a.ToCharArray();
            byte[] data = new byte[length];
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            string generatedString;
            StringBuilder strGeneratedString = new StringBuilder();
            // Generate the unique string
            foreach (byte b in data)
            {
                strGeneratedString.Append(chars[b % (chars.Length - 1)]);
            }
            // The generated unique string         
            generatedString = strGeneratedString.ToString();
            // Return the generated string
            return generatedString;
        }
       
    }
}
