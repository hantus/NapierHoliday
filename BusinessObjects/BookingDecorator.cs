using System;
using System.Collections.Generic;

namespace BusinessObjects
{

    /// <summary>
    /// Piotr Handkowski
    /// Abstract class used as a parent for 3 booking decorators. Contains implementation for methods and 
    /// properties from the parent class. A Decorator pattern has been used here; breakfast, evening meal
    /// and car hire are used to decorate a booking
    /// Last modified: 09/12/2017
    /// </summary>
    public abstract class BookingDecorator : AbstractBooking
    {

        private AbstractBooking _component; // represents a component to be decorated

        // method used to decorate a comporent
        public void SetComponent(AbstractBooking component)
        {
            this._component = component;
        }

        // method used to get the component of a decorator
        public AbstractBooking Component
        {
            get { return _component; }
            //set { _component = value; } IF NO ERRORS OCCUR - DELETE
        }

        // override of an inherited method. Returns number of guests for the booking
        public override int GuestCount()
        {
            return Component.GuestCount();
        }

        // override of an inherited method. Returns number of nights for the booking
        public override int NightCount()
        {
            return Component.NightCount();
        }

        // override of an inherited method. Returns booking ref number
        public override int BookingRefNo
        {
            get { return Component.BookingRefNo; }
            set { }
        }

        // override of an inherited method. Returns check in date
        public override DateTime ArrivalDate
        {
            get { return Component.ArrivalDate; }
            set { }
        }

        // override of an inherited method. Returns check in date as a string
        public override string ArrivalDateSt
        {
            get { return ArrivalDate.ToShortDateString(); }
        }

        // override of an inherited method. Returns departure date
        public override DateTime DepartureDate
        {
            get { return Component.DepartureDate; }
            set { }
        }

        // override of an inherited method. Returns departure date as a string
        public override string DepartureDateSt
        {
            get { return DepartureDate.ToShortDateString(); }
        }

        // override of an inherited method. Returns chalet id
        public override int ChaletId
        {
            get { return Component.ChaletId; }
            set { }
        }

        // override of an inherited method. Returns the customer for a booking
        public override Client Client
        {
            get { return Component.Client; }
            set { }
        }

        // override of an inherited method. Returns a list of guest for a booking
        public override List<Person> GuestList
        {
            get { return Component.GuestList; }
            set { }
        }

        //public override string CustomerName
        //{
        //    get { return Client.Name.ToString(); }
        //}

        // override of an inherited method. Returns total price for a booking
        public override double TotalPriece()
        {
            return CalculateCost() + Component.TotalPriece();
        }
    }
}
