using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;


namespace BankAppConsole
{
    /// <summary>
    /// Represents a Person opening an account
    /// </summary>
    public class Person
    {
        
        /// <summary>
        /// Defines name of the Customer
        /// </summary>
        private string PersonName;

        /// <summary>
        /// Defines the Gender of the Customer
        /// </summary>
        private Gender PersonGender;

        /// <summary>
        /// DEfines the customers date of birth
        /// </summary>
        private DateTime birthday;

        /// <summary>
        /// Defines the Customers phone number
        /// </summary>
        private string PhoneNumber;

        /// <summary>
        /// Defines the email address for contacting customer
        /// </summary>
        private string EmailAddress;

        /// <summary>
        /// Customer House Address
        /// </summary>
        private string HouseAddress;

        /// <summary>
        /// Customer Birthday
        /// </summary>
        public DateTime Birthday 
        {
            get { return birthday; }
            set { birthday = value; }
        }
        /// <summary>
        /// Gets or sets the name of Customer
        /// </summary>
        public string Name
        {
            get { return PersonName; }
            set { PersonName = value; }
        }

        /// <summary>
        /// Gets or sets the Customers gender to Male or Female.
        /// </summary>
        public Gender gender
        {
            get { return PersonGender; }
            set { PersonGender = value; }
        }

        /// <summary>
        /// Gets or sets the Customers phonenumber
        /// </summary>
        public string phonenumber
        {
            get { return PhoneNumber; }
            set { PhoneNumber = value; }
        }

        /// <summary>
        /// Gets or sets the Customers email-address
        /// </summary>
        public string email_address
        {
            get { return EmailAddress; }
            set { EmailAddress = value; }
        }

        /// <summary>
        /// Gets or sets the Customers residential-address
        /// </summary>
        public string house_address
        {
            get { return HouseAddress; }
            set { HouseAddress = value; }
        }

    }

    /// <summary>
    /// Possible genders of a person. (male or female).
    /// </summary>
    public enum Gender
    {
     /// <summary> The male gender </summary>
     Male,
     /// <summary> The female gender </summary>
     Female
    };
        
    
}

