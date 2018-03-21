using System;
using System.Collections.Generic;

namespace BusinessObjects
{

    /// <summary>
    /// Piotr Handkowski
    /// A sigleton factory class used to create booking objects. Depenting on the parameters passed a basic
    /// booking can be created as well as a booking that is decoratoed by up to 3 decorators.
    /// Last modified: 09/12/2017
    /// </summary>
    public class BookingFactorySingleton
    {
        private static BookingFactorySingleton _instance; // instance of the singleton class

        //private constructor prevents the user from creating new instances
        private BookingFactorySingleton()
        {
        }

        // a method that returns an instance of the class. If there is no instance then it creates a new
        // one, if there is it returns that instance
        public static BookingFactorySingleton Instance()
        {
            if (_instance == null)
            {
                _instance = new BookingFactorySingleton();
            }
            return _instance;
        }


        // the factory method creates an object which is a descendant of an AbstractBooking class. The created object is always of booking type
        // which can be decorated by up to 3 booking decorators
        public AbstractBooking FactoryMethod(int bookingRef, DateTime startDate, DateTime endDate, Client client, List<Person> guests,
            int chaletId, bool eveningMeal, bool breakfast, bool carHire, DateTime hireStart, DateTime hireEnd, string driver)
        {
            Booking booking = new Booking(bookingRef, startDate, endDate, client, guests, chaletId);
            EveningMealDecorator mealDecorator = null;
            BreakfastDecorator breakfastDecorator = null;
            CarHireDecorator carHireDecorator = null;
            if (eveningMeal == true)
            {
                mealDecorator = new EveningMealDecorator();
                mealDecorator.SetComponent(booking);

            }
            if (breakfast == true)
            {
                breakfastDecorator = new BreakfastDecorator();
                if (eveningMeal == true)
                {
                    breakfastDecorator.SetComponent(mealDecorator);
                }
                else
                {
                    breakfastDecorator.SetComponent(booking);
                }
            }
            if (carHire == true)
            {
                carHireDecorator = new CarHireDecorator();
                carHireDecorator.StartDate = hireStart;
                carHireDecorator.EndDate = hireEnd;
                carHireDecorator.Driver = driver;
                if (breakfast == true)
                {
                    carHireDecorator.SetComponent(breakfastDecorator);
                    return carHireDecorator;
                }
                if (eveningMeal == true)
                {
                    carHireDecorator.SetComponent(mealDecorator);
                    return carHireDecorator;
                }
                carHireDecorator.SetComponent(booking);
                return carHireDecorator;
            }
            if (breakfastDecorator != null)
            {
                return breakfastDecorator;
            }
            if (mealDecorator != null)
            {
                return mealDecorator;
            }
            return booking;
        }
    }
}
