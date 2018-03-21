using System;

namespace BusinessObjects
{

    /// <summary>
    /// Piotr Handkowski
    /// A class that is used to create customers that are a component of a booking.
    /// Last modified: 09/12/2017
    /// </summary>
    public class Client : Person
    {
        private string _address; // represents customer's address
        private int _customerNumber; // represents customer's number, it is provided by the database which prevents
                                     // from creating customers with the same number if the system is used on more
                                     // than one machine

        // 
        public Client(int customerNumber, string name, string address)
        {
            Name = name;
            Address = address;
            _customerNumber = customerNumber;
        }

        //public Client() { }  delete if not causing any problems

        // property for _address attribute. Includes validation for the setter
        public string Address
        {
            get { return _address; }
            set
            {
                if (value == String.Empty)
                {
                    throw new ArgumentNullException("Please provide address");
                }
                _address = value;
            }
        }

        // property for _customerNumber attribute. Inclueds validation
        public int CustomerNumber
        {
            get { return _customerNumber; }
            set
            {
                if (value < 1)
                {
                    throw new Exception("Customer number cannot be smaller than 1.");
                }
                _customerNumber = value;
            }
        }


    }
}
