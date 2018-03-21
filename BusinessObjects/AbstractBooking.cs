using System;
using System.Collections.Generic;

namespace BusinessObjects
{

    /// <summary>
    /// Piotr Handkowski
    /// Abstract parent class from which booking and booking decorators inherit. Contains methods and
    /// properties that the child classes inherit and implement. This allows access to bookings details
    /// through the decorators
    /// Last modified: 09/12/2017
    /// </summary>
    public abstract class AbstractBooking
    {
        // method used to calculate the cost of the element (basic booking, breakfast etc)
        public abstract double CalculateCost();
        // method used to get number of guests
        public abstract int GuestCount();
        // method used to get number of nights of the booking
        public abstract int NightCount();
        // method used to get the total price of the booking
        public abstract double TotalPriece();
        //property used to get /set booking reference number
        public abstract int BookingRefNo { get; set; }
        // property used to get/set check in date
        public abstract DateTime ArrivalDate { get; set; }
        // property used to get check in date as a string used in listView control
        public abstract string ArrivalDateSt { get; }
        // property used to get/set departure date
        public abstract DateTime DepartureDate { get; set; }
        // property used to get departure date as a string used in listView control
        public abstract string DepartureDateSt { get; }
        // property used to get/set chalet id
        public abstract int ChaletId { get; set; }
        // property used to get/set the client for a booking
        public abstract Client Client { get; set; }
        // property used to get/set guest list for a booking
        public abstract List<Person> GuestList { get; set; }


        // delete if not needed
        //public abstract string CustomerName { get; }

    }
}
