using System;
using System.Collections.Generic;

namespace BusinessObjects
{

    /// <summary>
    /// A facade singleton that provides all the method needed to create a booking
    /// Last modified: 09/12/2017
    /// </summary>
    public class BusinessFacadeSingleton
    {
        private static BusinessFacadeSingleton _instance; // an instance of the class

        // private constructor prevents from creating new objects
        private BusinessFacadeSingleton()
        {
        }

        // a method that creates the only instance of the class, or returns the instance if it has already been created
        public static BusinessFacadeSingleton Instance()
        {
            if (_instance == null)
            {
                _instance = new BusinessFacadeSingleton();
            }
            return _instance;
        }

        // a method that creates a new client from the data provided
        public Client CreateClient(int customerNumber, string name, string address)
        {
            return new Client(customerNumber, name, address);
        }

        // method that decorates a client as a guest
        public GuestDecorator CreateClientGuest(Client client, string passportNumber, int age)
        {
            GuestDecorator guest = new GuestDecorator(passportNumber, age);
            guest.SetComponent(client);
            return guest;
        }

        // a method that create a guest
        public GuestDecorator CreateGuest(string name, string passportNumber, int age)
        {
            GuestDecorator guest = new GuestDecorator(passportNumber, age);
            guest.Name = name;
            return guest;
        }

        // a method that creates whole booking from the details provided. It never returns AbstractBooking,
        // but one of it's descendants
        public AbstractBooking CreateBooking(int bookingRef, DateTime startDate, DateTime endDate, Client client, List<Person> guests,
            int chaletId, bool eveningMeal, bool breakfast, bool carHire, DateTime hireStart, DateTime hireEnd, string driver)
        {
            BookingFactorySingleton factory = BookingFactorySingleton.Instance();
            return factory.FactoryMethod(bookingRef, startDate, endDate, client, guests, chaletId, eveningMeal, breakfast, carHire, hireStart, hireEnd, driver);
        }
    }
}
