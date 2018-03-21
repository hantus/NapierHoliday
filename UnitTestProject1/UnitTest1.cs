using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BusinessObjects;
using System.Collections.Generic;

namespace UnitTestProject1
{
    /// <summary>
    /// Piotr Handkowski
    /// An unit test class testing methods and properties from Booking class
    /// Last modified: 09/12/2017
    /// </summary>
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void GuestCountTest()
        {
            //arrange
            List<Person> guests = new List<Person>();
            Person person = new Person();
            person.Name = "John";
            GuestDecorator guest = new GuestDecorator("ABC123456", 21);
            guest.SetComponent(person);
            guests.Add(guest);
            Client client = new Client(1, "Marry", "Edinburgh" );
            GuestDecorator guest2 = new GuestDecorator("XYZ987456", 25);
            guest.SetComponent(client);
            guests.Add(guest2);
            int expectedCount = 2;
            Booking booking = new Booking(1, DateTime.Today, DateTime.Today.Add(TimeSpan.FromDays(6)),
                client, guests, 2);
            BreakfastDecorator breakfastDecorator = new BreakfastDecorator();
            breakfastDecorator.SetComponent(booking);

            // act
            int actualCount = breakfastDecorator.GuestCount();

            //assert
            Assert.AreEqual(expectedCount, actualCount, 0, "GuestCount not working properly");
        }


        [TestMethod]
        public void NightCountTest()
        {
            //arrange
            List<Person> guests = new List<Person>();

            Client client = new Client(1, "Marry", "Edinburgh");
            GuestDecorator guest = new GuestDecorator("XYZ987456", 25);
            guest.SetComponent(client);
            guests.Add(guest);
            int expectedCount = 6;
            Booking booking = new Booking(1, DateTime.Today, DateTime.Today.Add(TimeSpan.FromDays(6)),
                client, guests, 2);
            EveningMealDecorator eveningMeal = new EveningMealDecorator();
            eveningMeal.SetComponent(booking);

            //act
            int actualCount = eveningMeal.NightCount();

            //assert
            Assert.AreEqual(expectedCount, actualCount, 0, "NightCount method not working properly");
        }

        [TestMethod]
        public void CalculateCost1()
        {
            //arrange
            List<Person> guests = new List<Person>();
            Person person = new Person();
            person.Name = "John";
            GuestDecorator guest = new GuestDecorator("ABC123456", 21);
            guest.SetComponent(person);
            guests.Add(guest);
            Client client = new Client(1, "Marry", "Edinburgh");
            GuestDecorator guest2 = new GuestDecorator("XYZ987456", 25);
            guest.SetComponent(client);
            guests.Add(guest2);
            double expectedAmount = 360;
            Booking booking = new Booking(1, DateTime.Today, DateTime.Today.Add(TimeSpan.FromDays(6)),
                client, guests, 2);


            //act
            double actualAmount = booking.CalculateCost();

            //assert
            Assert.AreEqual(expectedAmount, actualAmount, 0, "CalculateCost method not working properly");
        }


        [TestMethod]
        public void CalculateGuestSupplementTest()
        {
            //arrange
            List<Person> guests = new List<Person>();
            Person person = new Person();
            person.Name = "John";
            GuestDecorator guest = new GuestDecorator("ABC123456", 21);
            guest.SetComponent(person);
            guests.Add(guest);
            Client client = new Client(1, "Marry", "Edinburgh");
            GuestDecorator guest2 = new GuestDecorator("XYZ987456", 25);
            guest.SetComponent(client);
            guests.Add(guest2);
            double expectedAmount = 200;
            Booking booking = new Booking(1, DateTime.Today, DateTime.Today.Add(TimeSpan.FromDays(4)),
                client, guests, 2);


            //act
            double actualAmount = booking.CalculateGuestSupplement();

            //assert
            Assert.AreEqual(expectedAmount, actualAmount, 0, "CalculateCost method not working properly");
        }




        [TestMethod]
        public void TotalPriceTest1()
        {
            //arrange
            List<Person> guests = new List<Person>();

            Client client = new Client(1, "Marry", "Edinburgh");
            GuestDecorator guest = new GuestDecorator("XYZ987456", 25);
            guest.SetComponent(client);
            guests.Add(guest);
            double expectedAmount = 595;
            Booking booking = new Booking(1, DateTime.Today, DateTime.Today.Add(TimeSpan.FromDays(7)),
                client, guests, 2);


            // act
            double actualAmount = booking.TotalPriece();

            //assert
            Assert.AreEqual(expectedAmount, actualAmount, 0, "TotalPrice method working properly");
        }

        [TestMethod]
        public void TotalPriceTest2()
        {
            //arrange
            List<Person> guests = new List<Person>();
            Person person = new Person();
            person.Name = "John";
            GuestDecorator guest1 = new GuestDecorator("ABC123456", 21);
            guest1.SetComponent(person);
            guests.Add(guest1);
            Client client = new Client(1, "Marry", "Edinburgh");
            GuestDecorator guest = new GuestDecorator("XYZ987456", 25);
            guest.SetComponent(client);
            guests.Add(guest);
            double expectedAmount = 390;
            Booking booking = new Booking(1, DateTime.Today, DateTime.Today.Add(TimeSpan.FromDays(3)),
                client, guests, 2);
            EveningMealDecorator eveningMeal = new EveningMealDecorator();
            eveningMeal.SetComponent(booking);


            // act
            double actualAmount = eveningMeal.TotalPriece();

            //assert
            Assert.AreEqual(expectedAmount, actualAmount, 0, "TotalPrice method working properly");
        }

        [TestMethod]
        public void TotalPriceTest3()
        {
            //arrange
            List<Person> guests = new List<Person>();
            Client client = new Client(1, "Marry", "Edinburgh");
            GuestDecorator guest = new GuestDecorator("XYZ987456", 25);
            guest.SetComponent(client);
            guests.Add(guest);
            double expectedAmount = 580;
            Booking booking = new Booking(1, DateTime.Today, DateTime.Today.Add(TimeSpan.FromDays(4)),
                client, guests, 2);
            EveningMealDecorator eveningMeal = new EveningMealDecorator();
            eveningMeal.SetComponent(booking);
            CarHireDecorator carHireDecorator = new CarHireDecorator();
            carHireDecorator.StartDate = DateTime.Today;
            carHireDecorator.EndDate = DateTime.Today.Add(TimeSpan.FromDays(4));
            carHireDecorator.SetComponent(eveningMeal);


            // act
            double actualAmount = carHireDecorator.TotalPriece();

            //assert
            Assert.AreEqual(expectedAmount, actualAmount, 0, "TotalPrice method working properly");
        }
 

        [TestMethod]
        public void IncorrectDepartureDate_ShouldThrowArgumentException()
        {
            //arrange
            Booking booking = new Booking();
            booking.ArrivalDate = Convert.ToDateTime("21/12/2017");


            //act
            try
            {
                booking.DepartureDate = Convert.ToDateTime("20/12/2017");
            }
            catch (ArgumentException e)
            {
                //assert
                StringAssert.Contains(e.Message, "Please provide correct departure date");
                return;
            }
            Assert.Fail("No exception was thrown");

        }


        [TestMethod]
        public void BookingReferenceNumberZeroOrBelow_ShouldThrowArgumentException()
        {
            //arrange
            Booking booking = new Booking();



            //act
            try
            {
                booking.BookingRefNo = 0;
            }
            catch (ArgumentException e)
            {
                //assert
                StringAssert.Contains(e.Message, "Booking Reference number needs to be bigger than 0");
                return;
            }
            Assert.Fail("No exception was thrown");

        }

        [TestMethod]
        public void ChaletIdZeroOrBelow_ShouldThrowArgumentException()
        {
            //arrange
            Booking booking = new Booking();



            //act
            try
            {
                booking.ChaletId = -1;
            }
            catch (ArgumentException e)
            {
                //assert
                StringAssert.Contains(e.Message, "Chalet id can only be between 1 and 10");
                return;
            }
            Assert.Fail("No exception was thrown");

        }

        [TestMethod]
        public void ChaletIdOver10_ShouldThrowArgumentException()
        {
            //arrange
            Booking booking = new Booking();



            //act
            try
            {
                booking.ChaletId = 13;
            }
            catch (ArgumentException e)
            {
                //assert
                StringAssert.Contains(e.Message, "Chalet id can only be between 1 and 10");
                return;
            }
            Assert.Fail("No exception was thrown");

        }
    }
}
