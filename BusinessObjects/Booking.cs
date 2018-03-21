using System;
using System.Collections.Generic;

namespace BusinessObjects
{

    /// <summary>
    /// Piotr Handkowski
    /// Class used to create a basic booking to which extras(decorators) can be added. 
    /// Last modified: 09/12/2017
    /// </summary>
    public class Booking : AbstractBooking
    {

        private DateTime _arrivalDate; // represents check in date
        private DateTime _departureDate; // represents departure date
        private int _bookingReference; // represents booking ref number, it is assigned by the database
        private int _chaletId; // represents chalet number assigned to the booking (1-10)
        private Client _client; // represents the client that made the booking
        private List<Person> _guests;// represents a list of guests associated with the booking

        // a constructor that is used to create a new object. 
        public Booking(int bookingRef, DateTime startDate, DateTime endDate, Client client, List<Person> guests, int chaletId)
        {
            ArrivalDate = startDate;
            DepartureDate = endDate;
            GuestList = guests;
            Client = client;
            ChaletId = chaletId;
            BookingRefNo = bookingRef;// if a new object is constructed new number is provided by the database
                                      // which prevents duplicate numbers if the system was used on many machines
        }

        // basic constructor used in unit testing when testing single properties
        public Booking() { }

        // override of a property from parent class
        public override DateTime ArrivalDate
        {
            get { return _arrivalDate; }
            set { _arrivalDate = value; }
        }

        // override or a property from parent class.
        public override string ArrivalDateSt
        {
            get { return ArrivalDate.ToShortDateString(); }
        }

        // override or a property from parent class. Includes validation for the departure date
        public override DateTime DepartureDate
        {
            get { return _departureDate; }
            // add validation
            set
            {
                if (value <= this.ArrivalDate)
                {
                    throw new ArgumentOutOfRangeException("Please provide correct departure date");
                }
                _departureDate = value;
            }
        }

        // override or a property from parent class.
        public override string DepartureDateSt
        {
            get { return DepartureDate.ToShortDateString(); }
        }

        // override or a property from parent class. Includes validation for the booking ref number
        public override int BookingRefNo
        {
            get { return _bookingReference; }

            set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException("Booking Reference number needs to be bigger than 0.");
                }
                _bookingReference = value;

            }
        }

        // override or a property from parent class. Includes validation for the chaled id (1-10)
        public override int ChaletId
        {
            get { return _chaletId; }
            set
            {
                if (value < 1 || value > 10)
                {
                    throw new ArgumentOutOfRangeException("Chalet id can only be between 1 and 10");
                }
                _chaletId = value;
            }
        }

        // override or a property from parent class.
        public override Client Client
        {
            get { return _client; }
            set { _client = value; }
        }

        //public override string CustomerName
        //{
        //   get { return Client.Name.ToString(); }
        //}

        // override or a property from parent class.
        public override List<Person> GuestList
        {
            get { return _guests; }
            set { _guests = value; }

        }


        // override or a property from parent class.
        public override int GuestCount()
        {
            return _guests.Count;
        }

        // override or a property from parent class.
        public override int NightCount()
        {
            return (DepartureDate - ArrivalDate).Days;
        }

        //public void AddGuest(Person guest)
        //{
        //    this._guests.Add(guest);
        //}

        // override or a property from parent class. Returns the basic fee for the booking
        public override double CalculateCost()
        {
            return 60 * this.NightCount();
        }

        // override or a property from parent class. Returns the guest supplement fee for the booking
        public double CalculateGuestSupplement()
        {
            return (25 * _guests.Count) * this.NightCount();
        }

        // override or a property from parent class. Returns the total price for the booking
        public override double TotalPriece()
        {
            return CalculateCost() + CalculateGuestSupplement();
        }
    }
}
