using System;

namespace BusinessObjects
{
    /// <summary>
    /// Piotr Handkowski
    /// A class used to create a guest decorator. On its own it is used to create a guest that is associated with a booking.
    /// It can also be used to decorate a client in a situation when the client is also a guest.
    /// Last modified: 09/12/2017
    /// </summary>
    public class GuestDecorator : PersonDecorator
    {
        private string _passportNumber; // represents guest's passport number
        private int _age; // represents guest's age
        private Person _component; // represents the component, if used to decorate a client
        private string _name; // represents the guest's name

        // constructor
        public GuestDecorator(string passportNo, int age)
        {
            this.PassportNumber = passportNo;
            this.Age = age;
        }

        // property for _passportNumber attribute. Contains 2 validations
        public string PassportNumber
        {
            get { return _passportNumber; }
            set
            {
                if (value == String.Empty)
                {
                    throw new ArgumentNullException("Please provide passport number");
                }
                if (value.Length > 10)
                {
                    throw new Exception("Passport number can have 10 maximum 10 characters");
                }
                _passportNumber = value;
            }
        }

        // property for _age attribute. Contains a validation(0-101)
        public int Age
        {
            get { return _age; }
            set
            {
                if (value < 0 || value > 101)
                {
                    throw new ArgumentOutOfRangeException("Please provide age between 0 and 101");
                }
                _age = value;
            }
        }

        // property returning Person component
        public Person Component
        {
            get { return _component; }
        }

        // property for _name attribute. Contains 2 validations
        public override string Name
        {
            get
            {
                if (_component != null)
                {
                    return _component.Name;
                }
                else
                {
                    return _name;
                }
            }

            set
            {
                if (value == String.Empty)
                {
                    throw new ArgumentNullException("Please provide name");
                }
                _name = value;
            }
        }

        // method which sets the component
        public void SetComponent(Person component)
        {
            this._component = component;
        }

    }
}
