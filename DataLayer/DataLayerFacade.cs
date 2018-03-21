using System;
using System.Collections.Generic;
using System.Linq;
using BusinessObjects;

namespace DataLayer
{
    /// <summary>
    /// Piotr Handkowski
    /// A facade class that provides an interface for the presentation layer.
    /// </summary>
    public class DataLayerFacade
    {
        // savel all booking data in the database tables
        public static void SaveBooking(AbstractBooking booking, bool breakfast, bool eveningMeal)
        {
            DatabaseCalls.AddBooking(booking.BookingRefNo, booking.ArrivalDate, booking.DepartureDate,
                 booking.ChaletId, breakfast, eveningMeal);
            DatabaseCalls.CreateCustomer(booking.Client.CustomerNumber, booking.Client.Name,
                booking.Client.Address);
            DatabaseCalls.LinkClientToBooking(booking.BookingRefNo, booking.Client.CustomerNumber);
            foreach (var guest in booking.GuestList) // each guest is saved separately in the database
            {
                GuestDecorator guestDec = (GuestDecorator)guest;
                DatabaseCalls.CreateGuest(guestDec.Name, guestDec.PassportNumber,
                    guestDec.Age);
                DatabaseCalls.LinkGuestToBooking(booking.BookingRefNo, guestDec.PassportNumber);
                if (guestDec.Component != null)
                {
                    DatabaseCalls.LinkCustomerGuest(booking.Client.CustomerNumber, guestDec.PassportNumber);
                }
            }
            if (booking.GetType() == typeof(CarHireDecorator))
            {
                DatabaseCalls.AddCarHire(((CarHireDecorator)booking).Driver, booking.BookingRefNo,
                    ((CarHireDecorator)booking).StartDate, ((CarHireDecorator)booking).EndDate);
            }
        }

        // checks the database which chalets are occupied in the specified period and returns a list of available chalets
        public static List<int> AvailableChalets(DateTime arrivalDate, DateTime departureDate, int chaletId)
        {
            List<int> availableChalets = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            if (DatabaseCalls.OccupiedChalets(arrivalDate, departureDate, chaletId) != null)
            {
                foreach (int item in DatabaseCalls.OccupiedChalets(arrivalDate, departureDate, chaletId))
                {
                    availableChalets.Remove(item);
                }
            }
            return availableChalets;
        }

        // retreives the customer information from a database which and reconstructs a customer object from it
        public static List<Person> GetCustomer(int customerNumber)
        {
            List<Person> customerList = new List<Person>();
            List<CustomerItem> customerItemList = DatabaseCalls.GetCustomersDetails(customerNumber);
            if (customerItemList.Count == 0)
            {
                return customerList;
            }
            foreach (var item in customerItemList)
            {
                Client customer = new Client(item.Id, item.Name, item.Address);
                customerList.Add(customer);
            }

            return customerList;

        }

        // retreives details of all customers
        public static List<CustomerItem> GelAllCustomersInfo()
        {
            return DatabaseCalls.GetCustomersDetails(0);
        }

        // retreives a list of customers whose name matches the search details
        public static List<CustomerItem> FindCustomerByName(string name)
        {
            return DatabaseCalls.FindCustomerByName(name);
        }

        // retreives details of one customer whose name matches the search details
        public static CustomerItem GetOneCustomerDetails(string name)
        {
            return DatabaseCalls.GetSingleCustomerDetails(name);
        }

        // if a customer is also a guest it retreives a customer object wrapped in a guest decorator
        public static List<Person> GetGuestDecorator(int customerNmber, int bookingRef)
        {
            List<Person> guestList = new List<Person>();
            List<GuestItem> guestItemList = DatabaseCalls.GetGuestsDetails(customerNmber, bookingRef);
            if (guestItemList.Count == 0)
            {
                return guestList;
            }
            foreach (var item in guestItemList)
            {
                Person person = new Person { Name = item.Name };
                GuestDecorator guest = new GuestDecorator(item.PassportNumber, item.Age);
                guest.SetComponent(person);
                guestList.Add(guest);
            }
            return guestList;
        }

        // amends customer details held in the database
        public static void AmendCustomer(int custId, string name, string address)
        {
            DatabaseCalls.AmendCustomer(custId, name, address);
        }

        // amends guest details held in the database
        public static void AmendGuest(string name, string passportNumber, int age, string oldPassportNo)
        {
            DatabaseCalls.AmendGuest(name, passportNumber, age, oldPassportNo);
        }

        // checks for outstanding bookings for a customer
        public static int OutstandingBookings(int customerNumber)
        {
            int bookingNumber = DatabaseCalls.CheckForUpcomingBookings(customerNumber);
            return bookingNumber;
        }

        // retreives all information for a booking and reconstructs it into an object
        public static AbstractBooking RetreiveBooking(int bookingId)
        {
            Client customer = null;
            List<Person> guestList = new List<Person>();
            int customerRef = DatabaseCalls.GetCustIdForBooking(bookingId);
            CustomerItem customerItem = DatabaseCalls.GetCustomersDetails(customerRef).ElementAt(0);
            BusinessFacadeSingleton businessFacade = BusinessFacadeSingleton.Instance();
            customer = businessFacade.CreateClient(customerItem.Id, customerItem.Name, customerItem.Address);

            foreach (var guestItem in DatabaseCalls.GetGuestsDetails(0, bookingId))
            {
                GuestDecorator guest = businessFacade.CreateGuest(guestItem.Name, guestItem.PassportNumber, guestItem.Age);
                if (DatabaseCalls.IsCustomer(guest.Name))
                {
                    guest.SetComponent(customer);
                }
                guestList.Add(guest);
            }
            BookingItem bookingItem = DatabaseCalls.GetBookingDetails(bookingId);
            CarHireItem carHireItem = DatabaseCalls.GetCarHireDetails(bookingId);
            bool carHire = false;
            string driver = "";
            DateTime hireStart = DateTime.Today;
            DateTime hireEnd = DateTime.Today;
            if (carHireItem != null)
            {
                carHire = true;
                driver = carHireItem.Driver;
                hireStart = carHireItem.HireStart;
                hireEnd = carHireItem.HireEnd;
            }
            var booking = businessFacade.CreateBooking(bookingId, bookingItem.Arrival, bookingItem.Departure, customer, guestList,
            bookingItem.ChaletId, bookingItem.EveningMeal, bookingItem.Breakfast, carHire, hireStart,
            hireEnd, driver);

            return booking;
        }

        // deletes a guest from a booking
        public static void DeleteGuest(string passportNumber)
        {
            DatabaseCalls.DeleteGuest(passportNumber);
        }

        // adds a guest to an existing booking
        public static void AddGuestToBooking(int bookingRef, string name, string passportNumber, int age)
        {
            DatabaseCalls.CreateGuest(name, passportNumber, age);
            DatabaseCalls.LinkGuestToBooking(bookingRef, passportNumber);
        }

        // amends booking information held in the database
        public static void AmendBooking(int bookingRef, DateTime arrivalDate, DateTime departureDate,
            int chaletId, bool breakfast, bool eveningMeal)
        {
            DatabaseCalls.AmendBooking(bookingRef, arrivalDate, departureDate, chaletId, breakfast, eveningMeal);
        }

        // amends car hire details held in the database
        public static void AmendCarHire(string driver, int bookingRef, DateTime startDate, DateTime endDate)
        {
            DatabaseCalls.AmendCarHire(driver, bookingRef, startDate, endDate);
        }

        // adds car hire extra to an existing booking
        public static void AddCarHire(string driver, int bookingRef, DateTime startDate, DateTime endDate)
        {
            DatabaseCalls.AddCarHire(driver, bookingRef, startDate, endDate);
        }

        // removes car hire extra from an existing booking
        public static void RemoveCarHire(int bookingRef)
        {
            DatabaseCalls.DeleteCarHire(bookingRef);
        }

        // gives next booking ref number. Useful if the database is on a server and the system is used on many machines. It ensures that no
        // bookings with duplicate booking ref numbers are created.
        public static int GetNextBookingRef()
        {
            if (DatabaseCalls.GetNextBookingRef() == -1)
            {
                return 1;
            }
            return DatabaseCalls.GetNextBookingRef();
        }

        // gives next customer number. Useful if the database is on a server and the system is used on many machines. It ensures that no
        // customers with duplicate numbers are created.
        public static int GetNextCustomerNumber()
        {
            if (DatabaseCalls.GetNextCustomerNo() == -1)
            {
                return 1;
            }
            return DatabaseCalls.GetNextCustomerNo();
        }

        // retreives basic informations for all bookings used to display bookings info in a listView
        public static List<BasicBookingInfo> GetBasicBookingInfo()
        {
            return DatabaseCalls.GetBookingsInfo();
        }

        // deletes a customer
        public static void DeleteCustomer(int customerNumber)
        {
            DatabaseCalls.DeleteCustomer(customerNumber);
        }

        // deletes a booking
        public static void DeleteBooking(int bookingRef)
        {
            DatabaseCalls.DeleteGuestList(bookingRef);
            DatabaseCalls.DeleteCustomersReservation(bookingRef);
            DatabaseCalls.DeleteBooking(bookingRef);
        }
    }
}
