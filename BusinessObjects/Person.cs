using System;

namespace BusinessObjects
{
    /// <summary>
    /// Piotr Handkowski
    /// A parent class from which Client and PersonDecorator classes inherit
    /// Last modified:09/12/2017
    /// </summary>
    public class Person
    {
        private string _name; // represents the name of the person

        // property for the _name attribute. Contains one validation
        public virtual string Name
        {
            get { return _name; }
            set
            {
                if (value == String.Empty)
                {
                    throw new ArgumentNullException("Please provide name");
                }
                _name = value;
            }
        }
    }
}
