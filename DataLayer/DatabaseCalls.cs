using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;


namespace DataLayer
{
    /// <summary>
    /// Piotr Handkowski
    /// An inner class used to pass data received form database to listView of bookings
    /// Last modified: 09/12/2017
    /// </summary>
    public class BasicBookingInfo
    {
        public int Id { get; set; } // represents booking ref number 
        public string Name { get; set; } // represents the name of the customer
        public string Start { get; set; } // represents the check in date
        public string End { get; set; } // represents the departure date
    }

    public class CustomerItem
    {
        /// <summary>
        /// Piotr Handkowski
        /// An inner class used to pass data from database to a method that reconstructs the customer into objects
        /// Last modified: 09/12/2017
        /// </summary>
        public int Id { get; set; } // represents customer number
        public string Name { get; set; } // represents customer's name
        public string Address { get; set; } // represents customer's address
    }

    public class GuestItem
    {
        /// <summary>
        /// Piotr Handkowski
        /// An inner class used to pass data from database to a method that reconstructs it into guest objects
        /// Last modified: 09/12/2017
        /// </summary>
        public string Name { get; set; } // represents guest's name
        public string PassportNumber { get; set; } // represents guest's passport number
        public int Age { get; set; } // represents guest's age
    }

    public class BookingItem
    {
        /// <summary>
        /// Piotr Handkowski
        /// An inner class used to pass data from database to a method that reconstructs it into booking objects
        /// Last modified: 09/12/2017
        /// </summary>
        public int Id { get; set; }
        public DateTime Arrival { get; set; }
        public DateTime Departure { get; set; }
        public int ChaletId { get; set; }
        public bool Breakfast { get; set; }
        public bool EveningMeal { get; set; }
    }
    public class CarHireItem
    {
        /// <summary>
        /// Piotr Handkowski
        /// An inner class used to pass data from database to a method that reconstructs it into car hire objects
        /// Last modified: 09/12/2017
        /// </summary>
        public string Driver { get; set; }
        public DateTime HireStart { get; set; }
        public DateTime HireEnd { get; set; }
    }

    public class DatabaseCalls
    {


        /// <summary>
        /// Piotr Handkowski
        /// A class containing all methods used to save, update and retreive data from database. SQL queries have been written in one line
        /// as braking them up caused issues
        /// Last modified: 09/12/2017
        /// </summary>


        // ADD METHODS

        // enters a new row of data into Booking table. Data represents booking (without client and guests)
        public static void AddBooking(int bookingRef, DateTime ArrivalDate, DateTime DepartureDate,
            int chaletId, bool breakfast, bool eveningMeal)
        {
            string arrivalDate = ToDbDateConversion(ArrivalDate);
            string departureDate = ToDbDateConversion(DepartureDate);
            int breakfastBit = BoolToBit(breakfast);
            int eveningMealBit = BoolToBit(eveningMeal);
            try
            {
                using (SqlConnection con = Connect())
                {
                    con.Open();
                    string sql = String.Format(@"BEGIN TRANSACTION; insert into Booking values({0}, '{1}','{2}', {3}, {4}, {5}); COMMIT TRANSACTION;",
                        bookingRef, arrivalDate, departureDate, chaletId, breakfastBit, eveningMealBit);
                    SqlCommand com = new SqlCommand(sql, con);
                    com.ExecuteNonQuery();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // enters a new row of data into Customer table. 
        public static void CreateCustomer(int custId, string name, string address)
        {
            try
            {
                using (SqlConnection con = Connect())
                {
                    con.Open();
                    string sql = String.Format(@"BEGIN TRANSACTION; insert into Customer values({0}, '{1}','{2}'); COMMIT TRANSACTION; ",
                        custId, name, address);
                    SqlCommand com = new SqlCommand(sql, con);
                    com.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        // enters data into Customer_Reservation table. This table allows one customer to have multiple bookings
        public static void LinkClientToBooking(int bookingRef, int custId)
        {
            try
            {
                using (SqlConnection con = Connect())
                {
                    con.Open();
                    string sql = String.Format(@"BEGIN TRANSACTION; insert into Customers_Reservation values({0}, {1}); COMMIT TRANSACTION;",
                        bookingRef, custId);
                    SqlCommand com = new SqlCommand(sql, con);
                    com.ExecuteNonQuery();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // enters a new row of data into Guest table. 
        public static void CreateGuest(string name, string passportNumber, int age)
        {
            try
            {
                using (SqlConnection con = Connect())
                {
                    con.Open();
                    string sql = String.Format(@"BEGIN TRANSACTION; insert into Guest (Name, Passport_Number, Age) values('{0}', '{1}', {2}); COMMIT TRANSACTION; ",
                        name, passportNumber, age);
                    SqlCommand com = new SqlCommand(sql, con);
                    com.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        // enters a new row of data into Guest-List table. This allows likning guests to bookings 
        public static void LinkGuestToBooking(int bookingRef, string passportNumber)
        {
            try
            {
                using (SqlConnection con = Connect())
                {
                    con.Open();
                    string sql = String.Format(@"BEGIN TRANSACTION;  insert into Guest_List values({0},( SELECT Guest_Number FROM Guest WHERE Passport_Number = '{1}')); COMMIT TRANSACTION; ",
                        bookingRef, passportNumber);
                    SqlCommand com = new SqlCommand(sql, con);
                    com.ExecuteNonQuery();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // enters a new row of data into Staying_Customer table. It is used when customer is also a guest
        public static void LinkCustomerGuest(int custId, string passportNumber)
        {
            try
            {
                using (SqlConnection con = Connect())
                {
                    con.Open();
                    string sql = String.Format(@"BEGIN TRANSACTION; insert into Staying_Customer values({0}, (SELECT Guest_Number FROM Guest WHERE Passport_Number = '{1}')); COMMIT TRANSACTION; ",
                        custId, passportNumber);
                    SqlCommand com = new SqlCommand(sql, con);
                    com.ExecuteNonQuery();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // enters a new row of data into Car_Hire table. 
        public static void AddCarHire(string driver, int bookingRef, DateTime startDate, DateTime endDate)
        {
            string hireStartDate = ToDbDateConversion(startDate);
            string hireEndDate = ToDbDateConversion(endDate);

            try
            {
                using (SqlConnection con = Connect())
                {
                    con.Open();
                    string sql = String.Format(@"BEGIN TRANSACTION; insert into Car_Hire values('{0}', {1}, '{2}', '{3}'); COMMIT TRANSACTION; ",
                       driver, bookingRef, hireStartDate, hireEndDate);
                    SqlCommand com = new SqlCommand(sql, con);
                    com.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // DELETE METHODS

        // deletes customer record from the database
        public static void DeleteCustomer(int custId)
        {
            try
            {
                using (SqlConnection con = Connect())
                {
                    con.Open();
                    string sql = String.Format(" BEGIN TRANSACTION; DELETE FROM Staying_Customer WHERE Customer_Id = {0}; DELETE FROM Customers_Reservation  WHERE Customer_Id = {0}; DELETE FROM Customer WHERE Customer_Id = {0}; COMMIT TRANSACTION;",
                        custId);
                    SqlCommand com = new SqlCommand(sql, con);
                    com.ExecuteNonQuery();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // deletes guest from the database
        public static void DeleteGuest(string passportNumber)
        {
            try
            {
                using (SqlConnection con = Connect())
                {

                    con.Open();
                    string sql = String.Format("BEGIN TRANSACTION; DELETE FROM Guest_List WHERE Guest_Number = (SELECT Guest_Number FROM Guest WHERE Passport_Number = '{0}');" +
                        "DELETE FROM Staying_Customer WHERE Guest_Number = (SELECT Guest_Number FROM Guest WHERE Passport_Number = '{0}');" +
                        "DELETE FROM Guest WHERE Passport_Number = '{0}'; COMMIT TRANSACTION; ",
                        passportNumber);
                    SqlCommand com = new SqlCommand(sql, con);
                    com.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // deletes customer reservation from a bridge table
        public static void DeleteCustomersReservation(int bookingRef)
        {
            try
            {
                using (SqlConnection con = Connect())
                {
                    con.Open();
                    string sql = String.Format("BEGIN TRANSACTION; DELETE FROM Customers_Reservation WHERE Booking_Ref = '{0}'; COMMIT TRANSACTION;",
                        bookingRef);
                    SqlCommand com = new SqlCommand(sql, con);
                    com.ExecuteNonQuery();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // deletes guest from a guest list for a particular booking
        public static void DeleteGuestList(int bookingRef)
        {
            try
            {
                using (SqlConnection con = Connect())
                {
                    con.Open();
                    string sql = String.Format("BEGIN TRANSACTION; DELETE FROM Guest_List WHERE Booking_Ref = '{0}'; COMMIT TRANSACTION;",
                        bookingRef);
                    SqlCommand com = new SqlCommand(sql, con);
                    com.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // deletes car hire extra from a booking
        public static void DeleteCarHire(int bookingRef)
        {
            try
            {
                using (SqlConnection con = Connect())
                {
                    con.Open();
                    string sql = String.Format("BEGIN TRANSACTION; DELETE FROM Car_Hire WHERE Booking_Ref = '{0}'; COMMIT TRANSACTION;",
                        bookingRef);
                    SqlCommand com = new SqlCommand(sql, con);
                    com.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // deletes booking from the database
        public static void DeleteBooking(int bookingRef)
        {
            try
            {
                using (SqlConnection con = Connect())
                {
                    con.Open();
                    string sql = String.Format("BEGIN TRANSACTION; DELETE FROM Booking WHERE RefNo = '{0}'; COMMIT TRANSACTION;",
                        bookingRef);
                    SqlCommand com = new SqlCommand(sql, con);
                    com.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // AMEND METHODS


        // amends customer details held in the database
        public static void AmendCustomer(int custId, string name, string address)
        {
            try
            {
                using (SqlConnection con = Connect())
                {
                    con.Open();
                    string sql = String.Format("BEGIN TRANSACTION; UPDATE Customer SET Name = '{1}', Address = '{2}'  WHERE Customer_Id = {0}; COMMIT TRANSACTION; ",
                        custId, name, address);
                    SqlCommand com = new SqlCommand(sql, con);
                    com.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
        }

        // amends booking details held in the database
        public static void AmendBooking(int bookingRef, DateTime ArrivalDate, DateTime DepartureDate,
            int chaletId, bool breakfast, bool eveningMeal)
        {
            string arrivalDate = ToDbDateConversion(ArrivalDate);
            string departureDate = ToDbDateConversion(DepartureDate);
            int breakfastBit = BoolToBit(breakfast);
            int eveningMealBit = BoolToBit(eveningMeal);
            try
            {
                using (SqlConnection con = Connect())
                {
                    con.Open();
                    string sql = String.Format("BEGIN TRANSACTION; UPDATE Booking SET Arrival_Date = '{1}', Departure_Date = '{2}',Chalet_Id = {3}, Breakfast = {4}, Evening_Meal = {5} WHERE RefNo = '{0}'; COMMIT TRANSACTION; ",
                        bookingRef, arrivalDate, departureDate, chaletId, breakfastBit, eveningMealBit);
                    SqlCommand com = new SqlCommand(sql, con);
                    com.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
        }

        // amends guest details held in the database
        public static void AmendGuest(string name, string passportNumber, int age, string oldPassport)
        {
            try
            {
                using (SqlConnection con = Connect())
                {
                    con.Open();
                    string sql = String.Format("BEGIN TRANSACTION; UPDATE Guest SET Name = '{0}',Passport_Number ='{1}', Age = {2} WHERE Passport_Number = '{3}'; COMMIT TRANSACTION;",
                        name, passportNumber, age, oldPassport);
                    SqlCommand com = new SqlCommand(sql, con);
                    com.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
        }

        // amends car hire details held in the database
        public static void AmendCarHire(string driver, int bookingRef, DateTime startDate, DateTime endDate)
        {
            string hireStartDate = ToDbDateConversion(startDate);
            string hireEndDate = ToDbDateConversion(endDate);
            try
            {
                using (SqlConnection con = Connect())
                {
                    con.Open();
                    string sql = String.Format("BEGIN TRANSACTION; UPDATE Car_Hire SET Drivers_Name = '{0}', Start_Date = '{2}', End_Date = '{3}' WHERE Booking_ref = {1}; COMMIT TRANSACTION;",
                        driver, bookingRef, hireStartDate, hireEndDate);
                    SqlCommand com = new SqlCommand(sql, con);
                    com.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
        }


        // FIND METHODS


        // retreives booking information that is displayed in listView window
        public static List<BasicBookingInfo> GetBookingsInfo()
        {
            try
            {
                using (SqlConnection con = Connect())
                {
                    con.Open();
                    string sql = String.Format("Select RefNo, Name, Arrival_Date, Departure_Date" +
                        @" FROM Booking JOIN Customers_Reservation ON (RefNo = Booking_Ref) JOIN Customer " +
                        @"ON (Customer.Customer_Id = Customers_Reservation.Customer_Id);");
                    SqlCommand com = new SqlCommand(sql, con);
                    SqlDataReader sdr = com.ExecuteReader();
                    List<BasicBookingInfo> bookingList = new List<BasicBookingInfo>();

                    while (sdr.Read())
                    {
                        {
                            BasicBookingInfo item = new BasicBookingInfo();
                            item.Id = sdr.GetInt32(0);
                            item.Name = sdr.GetString(1);
                            item.Start = sdr.GetDateTime(2).ToShortDateString();
                            item.End = sdr.GetDateTime(3).ToShortDateString();
                            bookingList.Add(item);
                        }
                    }
                    return bookingList;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        // retreives details of 1 or many customers depending on the parameter provided. If provided with a customer number
        // it will get details of this customer. If 0 is passed it will get a list of all customers
        public static List<CustomerItem> GetCustomersDetails(int custNumber)
        {
            try
            {
                using (SqlConnection con = Connect())
                {

                    con.Open();
                    string sql;
                    if (custNumber == 0)
                    {
                        sql = String.Format("SELECT * FROM Customer;");
                    }
                    else
                    {
                        sql = String.Format("SELECT * FROM Customer WHERE Customer_Id = {0};",
                            custNumber);
                    }

                    SqlCommand com = new SqlCommand(sql, con);
                    SqlDataReader sdr = com.ExecuteReader();
                    List<CustomerItem> customerList = new List<CustomerItem>();

                    while (sdr.Read())
                    {
                        {
                            CustomerItem customer = new CustomerItem();
                            customer.Id = sdr.GetInt32(0);
                            customer.Name = sdr.GetString(1);
                            customer.Address = sdr.GetString(2);
                            customerList.Add(customer);
                        }
                    }
                    return customerList;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }


        // retreives guest details 
        public static CustomerItem GetSingleCustomerDetails(string name)
        {
            try
            {
                using (SqlConnection con = Connect())
                {

                    con.Open();
                    string sql = String.Format("SELECT * FROM Customer WHERE Name = '{0}';",
                            name);
                    SqlCommand com = new SqlCommand(sql, con);
                    SqlDataReader sdr = com.ExecuteReader();
                    CustomerItem customer = new CustomerItem();
                    while (sdr.Read())
                    {
                        {
                            customer.Id = sdr.GetInt32(0);
                            customer.Name = sdr.GetString(1);
                            customer.Address = sdr.GetString(2);
                        }
                    }
                    return customer;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        // retreives customer id associated with a booking
        public static int GetCustIdForBooking(int bookingRef)
        {
            try
            {
                using (SqlConnection con = Connect())
                {
                    con.Open();
                    string sql = String.Format("SELECT Customer_Id FROM Customers_Reservation WHERE Booking_Ref = {0};",
                        bookingRef);
                    SqlCommand com = new SqlCommand(sql, con);
                    SqlDataReader sdr = com.ExecuteReader();
                    int customerId = -1;
                    while (sdr.Read())
                    {
                        customerId = sdr.GetInt32(0);
                    }
                    return customerId;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }
        }

        // retreives guest getails. If customer number is provided it will retreive the guest details if the customer is also a guest. Otherwise
        // it will retreive a list of guest for the booking ref number
        public static List<GuestItem> GetGuestsDetails(int custNumber, int bookingRef)
        {
            List<GuestItem> guestList = new List<GuestItem>();
            try
            {
                using (SqlConnection con = Connect())
                {
                    con.Open();
                    string sql;
                    if (custNumber == 0)
                    {
                        sql = String.Format("SELECT Name, Passport_Number, Age FROM Guest JOIN Guest_List ON (Guest_List.Guest_Number = Guest.Guest_Number) WHERE Guest_List.Booking_Ref = {0};", bookingRef);
                    }
                    else
                    {
                        sql = String.Format("SELECT  Guest.Name, Guest.Passport_Number, Guest.Age FROM Guest JOIN Staying_Customer ON (Staying_Customer.Guest_Number = Guest.Guest_Number) JOIN Customer ON (Staying_Customer.Customer_Id = Customer.Customer_Id) WHERE Customer.Customer_Id = {0};",
                            custNumber);
                    }
                    SqlCommand com = new SqlCommand(sql, con);
                    SqlDataReader sdr = com.ExecuteReader();
                    while (sdr.Read())
                    {
                        GuestItem guest = new GuestItem();
                        guest.Name = sdr.GetString(0);
                        guest.PassportNumber = sdr.GetString(1);
                        guest.Age = sdr.GetInt32(2);
                        guestList.Add(guest);
                    }
                    return guestList;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return guestList;
            }
        }

        // retreives booking information that are used to reconstruct booking object
        public static BookingItem GetBookingDetails(int bookingRef)
        {
            BookingItem bookingItem = new BookingItem();
            try
            {
                using (SqlConnection con = Connect())
                {
                    con.Open();
                    string sql = String.Format("SELECT * FROM Booking WHERE   RefNo = {0};",
                           bookingRef);
                    SqlCommand com = new SqlCommand(sql, con);
                    SqlDataReader sdr = com.ExecuteReader();
                    while (sdr.Read())
                    {
                        {
                            bookingItem.Id = bookingRef;
                            bookingItem.Arrival = sdr.GetDateTime(1);
                            bookingItem.Departure = sdr.GetDateTime(2);
                            bookingItem.ChaletId = sdr.GetInt32(3);
                            bookingItem.Breakfast = sdr.GetBoolean(4);
                            bookingItem.EveningMeal = sdr.GetBoolean(5);
                        }
                    }
                    return bookingItem;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return bookingItem;
            }
        }


        // retreives car hire information that for a booking
        public static CarHireItem GetCarHireDetails(int bookingRef)
        {
            CarHireItem carHireItem = null;
            try
            {
                using (SqlConnection con = Connect())
                {
                    con.Open();
                    string sql = String.Format("SELECT * FROM Car_Hire WHERE   Booking_Ref = {0};",
                           bookingRef);
                    SqlCommand com = new SqlCommand(sql, con);
                    SqlDataReader sdr = com.ExecuteReader();
                    while (sdr.Read())
                    {
                        {
                            carHireItem = new CarHireItem();
                            carHireItem.Driver = sdr.GetString(0);
                            carHireItem.HireStart = sdr.GetDateTime(2);
                            carHireItem.HireEnd = sdr.GetDateTime(3);
                        }
                    }
                    return carHireItem;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return carHireItem;
            }
        }

        // checks if a guest is also a customer used when reconstructing a booking. If returns true then client is set as a component 
        // of guest decorator
        public static bool IsCustomer(string guestName)
        {
            bool isCustomer = false;
            try
            {
                using (SqlConnection con = Connect())
                {
                    con.Open();
                    string sql = String.Format("SELECT * FROM Staying_Customer JOIN Guest ON (Guest.Guest_Number = Staying_Customer.Guest_Number) WHERE Guest.Name = '{0}';",
                           guestName);
                    SqlCommand com = new SqlCommand(sql, con);
                    SqlDataReader sdr = com.ExecuteReader();
                    while (sdr.Read())
                    {
                        isCustomer = true;
                    }
                    return isCustomer;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return isCustomer;
            }
        }

        // finds all customers when provided a part of their name. Used when looking for existing customers by name
        public static List<CustomerItem> FindCustomerByName(string name)
        {
            try
            {
                using (SqlConnection con = Connect())
                {
                    con.Open();
                    string sql = String.Format("SELECT Name, Address FROM Customer WHERE Name LIKE '%{0}%';", name);
                    SqlCommand com = new SqlCommand(sql, con);
                    SqlDataReader sdr = com.ExecuteReader();
                    List<CustomerItem> customerList = new List<CustomerItem>();
                    while (sdr.Read())
                    {
                        {
                            CustomerItem customerItem = new CustomerItem();
                            customerItem.Name = sdr.GetString(0);
                            customerItem.Address = sdr.GetString(1);
                            customerList.Add(customerItem);
                        }
                    }
                    return customerList;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        // returns a list of occupied chalest in the period provided.
        public static List<int> OccupiedChalets(DateTime arrivalDate, DateTime departureDate, int chaletId)
        {
            try
            {
                using (SqlConnection con = Connect())
                {
                    con.Open();
                    string startDate = ToDbDateConversion(arrivalDate);
                    string endDate = ToDbDateConversion(departureDate);
                    string sql;
                    if (chaletId == 0)
                    {
                        sql = String.Format("SELECT DISTINCT Chalet_Id FROM Booking WHERE (Arrival_Date >= '{0}' AND Arrival_Date < '{1}') OR (Departure_Date > '{0}' AND Departure_Date <= '{1}') OR (Arrival_Date < '{0}' AND Departure_Date> '{1}');",
                            startDate, endDate);
                    }
                    else
                    {
                        sql = String.Format("SELECT DISTINCT Chalet_Id FROM Booking WHERE ((Arrival_Date >= '{0}' AND Arrival_Date < '{1}') OR (Departure_Date > '{0}' AND Departure_Date <= '{1}') OR (Arrival_Date < '{0}' AND Departure_Date> '{1}')) AND Chalet_Id <> {2} ;",
                            startDate, endDate, chaletId);
                    }
                    SqlCommand com = new SqlCommand(sql, con);
                    SqlDataReader sdr = com.ExecuteReader();
                    List<int> occupiedChalets = new List<int>();
                    while (sdr.Read())
                    {
                        {
                            int bookedChalet = sdr.GetInt32(0);
                            occupiedChalets.Add(bookedChalet);
                        }
                    }
                    return occupiedChalets;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        // checks if a customer has outstanding bookings. Used when you want to delete a customer
        public static int CheckForUpcomingBookings(int customerNumber)
        {
            int numberOfBookings = 0;
            try
            {
                using (SqlConnection con = Connect())
                {
                    con.Open();
                    string sql = String.Format("SELECT COUNT(*) FROM Customers_Reservation JOIN Booking ON (Booking_Ref = RefNo AND Customer_Id = {0}) WHERE Departure_Date > GETDATE();",
                        customerNumber);
                    SqlCommand com = new SqlCommand(sql, con);
                    SqlDataReader sdr = com.ExecuteReader();
                    while (sdr.Read())
                    {
                        {
                            numberOfBookings = sdr.GetInt32(0);
                        }
                    }
                    return numberOfBookings;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return numberOfBookings;
            }
        }

        // gives next booking ref number. Useful if the database is on a server and the system is used on many machines. It ensures that no
        // bookings with duplicate booking ref numbers are created.
        public static int GetNextBookingRef()
        {
            int nextBookRef = 0;
            try
            {
                using (SqlConnection con = Connect())
                {
                    con.Open();
                    string sql = String.Format("SELECT MAX(RefNo) FROM Booking;");
                    SqlCommand com = new SqlCommand(sql, con);
                    SqlDataReader sdr = com.ExecuteReader();
                    while (sdr.Read())
                    {
                        {
                            nextBookRef = sdr.GetInt32(0);
                        }
                    }
                    return nextBookRef + 1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }
        }

        // gives next customer number. Useful if the database is on a server and the system is used on many machines. It ensures that no
        // customers with duplicate numbers are created.
        public static int GetNextCustomerNo()
        {
            int nextCustomerId = 0;
            try
            {
                using (SqlConnection con = Connect())
                {
                    con.Open();
                    string sql = String.Format("SELECT MAX(Customer_Id) FROM Customer;");
                    SqlCommand com = new SqlCommand(sql, con);
                    SqlDataReader sdr = com.ExecuteReader();
                    while (sdr.Read())
                    {
                        {
                            nextCustomerId = sdr.GetInt32(0);
                        }
                    }
                    return nextCustomerId + 1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }
        }



        //------------- helper methods


        // creates a connection to a database
        private static SqlConnection Connect()
        {
            SqlConnection con = new SqlConnection(
                @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|BookingSystem.mdf;Integrated Security=True");

            return con;
        }

        // converts DateTime format to a database date format
        private static string ToDbDateConversion(DateTime date)
        {
            string dateToConvert = date.ToShortDateString();
            string convertedDate = dateToConvert.Substring(6, 4) + "/" +
                                   dateToConvert.Substring(3, 2) + "/" +
                                   dateToConvert.Substring(0, 2);
            return convertedDate;
        }


        // converts bool into database bit format
        private static int BoolToBit(bool boolean)
        {
            if (boolean == true)
            {
                return 1;
            }
            return 0;
        }


    }
}