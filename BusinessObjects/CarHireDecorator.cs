using System;

namespace BusinessObjects
{

    /// <summary>
    /// Piotr Handkowski
    /// A car hire decorator class used to create a booking extra (car hire) which is then used to decorate
    /// a booking.
    /// Last modified: 09/12/2017
    /// </summary>
    public class CarHireDecorator : BookingDecorator
    {
        private DateTime _startDate; // represents the car hire start date
        private DateTime _endDate; // represents the car hire end date
        private string _driver; // represents the driver

        // propertrty for _startDate attribute. 
        public DateTime StartDate
        {
            get { return _startDate; }
            set { _startDate = value; }
        }

        // property for _endDate attribute
        public DateTime EndDate
        {
            get { return _endDate; }
            set { _endDate = value; }
        }

        // property for _driver property
        public string Driver
        {
            get { return _driver; }
            set
            {
                if(value == String.Empty)
                {
                    throw new Exception("Please provide driver's name");
                }
                _driver = value;
            }
        }

        // override on an inherited method. Returns the cost of car hire for the booking
        public override double CalculateCost()
        {
            return 50 * (EndDate - StartDate).TotalDays;
        }
    }
}
