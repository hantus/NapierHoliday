using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BusinessObjects;
using DataLayer;

namespace Presentation
{
    /// <summary>
    /// Piotr Handkowski
    /// Interaction logic for BookingWindow.xaml
    /// Last modified 09/12/2017
    /// </summary>
    public partial class BookingWindow : Window
    {

        private List<Person> guests = new List<Person>(); // global list that will hold details of guests
        private Client client = null; // global client 
        private Client existingClient = null; // globla existing client. For multiple booking
        public BookingWindow()
        {
            InitializeComponent();
            Application.Current.MainWindow = this;
            Application.Current.MainWindow.Height = 200;
            txtBoxGuestPasspNo.MaxLength = 10;
            txtBoxCustName.MaxLength = 50;
            txtBoxGuestName.MaxLength = 50;
            txtBoxCustAddress.MaxLength = 150;
            HideFields();
        }

        // logic for check availability button. Returns a list of chalets that are available during the period specified
        private void btnCheckAvailability_Click(object sender, RoutedEventArgs e)
        {
            if (datePickerStartDate.SelectedDate == null)
            {
                MessageBox.Show("Please provide Check In date");
                return;
            }
            if (datePickerEndDate.SelectedDate == null)
            {
                MessageBox.Show("Please provide Departure date");
                return;
            }
            if (datePickerStartDate.SelectedDate >= datePickerEndDate.SelectedDate)
            {
                MessageBox.Show("Please note that departure date must be after arrival date");
                return;
            }
            DateTime startDate = datePickerStartDate.SelectedDate.Value.Date;
            DateTime endDate = datePickerEndDate.SelectedDate.Value.Date;
            datePickerCarFrom.DisplayDateStart = startDate;
            datePickerCarFrom.DisplayDateEnd = endDate.Subtract(TimeSpan.FromDays(1));
            datePickerCarTo.DisplayDateStart = startDate.Add(TimeSpan.FromDays(1));
            datePickerCarTo.DisplayDateEnd = endDate;
            List<int> availableChalets = DataLayerFacade.AvailableChalets(startDate, endDate, 0);
            if (availableChalets.Count == 0)
            {
                MessageBox.Show("There are no chalets available during this period. Please choose different dates");
                return;
            }
            else
            {
                MessageBox.Show("There are " + availableChalets.Count + " chalets available");
                comBoxChaletId.Items.Clear(); // Clears prevoius items
                foreach (var chalet in availableChalets)
                {
                    comBoxChaletId.Items.Add(chalet);
                }

                ShowFields();
                Application.Current.MainWindow.Height = 650;
            }
        }

        // populates guest details if client is also a guest
        private void checkBoxGuest_Checked(object sender, RoutedEventArgs e)
        {
            txtBoxGuestName.Text = txtBoxCustName.Text;
            if (checkBoxExistCust.IsChecked == true)
            {
                // for an existing customer a check is done if he/she was also a guest so the 
                // guest details can be used in a new booking
                var selectedCustomer = DataLayerFacade.GetOneCustomerDetails(txtBoxCustName.Text);
                var guestDetailsList = DataLayerFacade.GetGuestDecorator(selectedCustomer.Id, 0);
                if (guestDetailsList.Count != 0)
                {
                    GuestDecorator guest = (GuestDecorator)guestDetailsList.ElementAt(0);
                    txtBoxGuestAge.Text = guest.Age.ToString();
                    txtBoxGuestPasspNo.Text = guest.PassportNumber;
                }

            }
        }

        // clears guest details if add customer as guest box is unchecked
        private void checkBoxGuest_Unchecked(object sender, RoutedEventArgs e)
        {
            txtBoxGuestName.Text = "";
            txtBoxGuestPasspNo.Text = "";
            txtBoxGuestAge.Text = "";
        }

        // logic for add guest button. Adds a guest to guest list
        private void btnAddGuest_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtBoxGuestName.Text.Equals(String.Empty) ||
                txtBoxGuestAge.Text.Equals(String.Empty) ||
                txtBoxGuestPasspNo.Text.Equals(String.Empty))
                {
                    MessageBox.Show("Please provide all guest's details");
                    return;
                }
                string guestName = txtBoxGuestName.Text;
                string passportNo = txtBoxGuestPasspNo.Text;
                int age;
                try
                {
                    age = Convert.ToInt32(txtBoxGuestAge.Text);
                }
                catch (Exception)
                {
                    MessageBox.Show("Please provide age up to 101 as a number.");
                    return;
                }

                string address = txtBoxCustAddress.Text;
                var facade = BusinessFacadeSingleton.Instance();
                GuestDecorator guest = null;
                // below code checks retreives details of an existing customer(s) if an existing customer
                // wished to make a new booking
                if (checkBoxAddAsGuest.IsChecked == true && checkBoxExistCust.IsChecked == false)
                {
                    if (txtBoxCustName.Text == String.Empty || txtBoxCustAddress.Text == String.Empty)
                    {
                        MessageBox.Show("Please provide customer details.");
                        return;
                    }
                    int customerNumber = DataLayerFacade.GetNextCustomerNumber();
                    client = facade.CreateClient(customerNumber, guestName, address);
                    guest = facade.CreateClientGuest(client, passportNo, age);
                }
                else
                {
                    guest = facade.CreateGuest(guestName, passportNo, age);
                    if (client == null)
                    {
                        // checks if the existing customer was also a guest so the guest details can be used
                        if (checkBoxExistCust.IsChecked == true)
                        {
                            var existingCustomerDetails = DataLayerFacade.GetOneCustomerDetails(txtBoxCustName.Text);
                            existingClient = new Client(existingCustomerDetails.Id, existingCustomerDetails.Name, existingCustomerDetails.Address);
                            if (existingClient == null)
                            {
                                MessageBox.Show("Please provide client's details.");
                                return;
                            }
                            client = existingClient;
                        }
                        else
                        {
                            if (txtBoxCustName.Text == String.Empty || txtBoxCustAddress.Text == String.Empty)
                            {
                                MessageBox.Show("Please provide customer details.");
                                return;
                            }
                            int customerNumber = DataLayerFacade.GetNextCustomerNumber();
                            string clientName = txtBoxCustName.Text;
                            string clinetAddress = txtBoxCustAddress.Text;
                            client = facade.CreateClient(customerNumber, clientName, clinetAddress);
                        }

                    }
                }
                guests.Add(guest);
                if (guests.Count == 6)
                {
                    MessageBox.Show("You have reached the maxumum number of guests");
                    txtBoxGuestName.IsReadOnly = true;
                    txtBoxGuestAge.IsReadOnly = true;
                    txtBoxGuestPasspNo.IsReadOnly = true;
                }

                listBoxGuestList.Items.Add(guestName);
                comBoxDriver.Items.Add(guestName);// ads the name to a combo box so it can be selected as a driver
                txtBoxGuestName.Text = "";
                txtBoxGuestPasspNo.Text = "";
                txtBoxGuestAge.Text = "";
                checkBoxAddAsGuest.IsChecked = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        // shows car hire controls when the checkbox is selected
        private void checkBoxCarHire_Checked(object sender, RoutedEventArgs e)
        {
            lblCarHiFrom.Visibility = Visibility.Visible;
            lblCarHiTo.Visibility = Visibility.Visible;
            datePickerCarFrom.Visibility = Visibility.Visible;
            datePickerCarTo.Visibility = Visibility.Visible;
            lblDriver.Visibility = Visibility.Visible;
            comBoxDriver.Visibility = Visibility.Visible;
        }

        // hides car hire controls when the checkbox is unchecked
        private void checkBoxCarHire_Unchecked(object sender, RoutedEventArgs e)
        {
            lblCarHiFrom.Visibility = Visibility.Hidden;
            lblCarHiTo.Visibility = Visibility.Hidden;
            datePickerCarFrom.Visibility = Visibility.Hidden;
            datePickerCarTo.Visibility = Visibility.Hidden;
            lblDriver.Visibility = Visibility.Hidden;
            comBoxDriver.Visibility = Visibility.Hidden;
        }

        // logic for confirm booking button. Creates a booking object and saves all its details in the database
        private void btnConfirmBooking_Click(object sender, RoutedEventArgs e)
        {
            if (guests.Count == 0)
            {
                MessageBox.Show("Please enter guests.");
                return;
            }
            if (comBoxChaletId.SelectedItem == null)
            {
                MessageBox.Show("Please choose a chalet.");
                return;
            }
            if (checkBoxCarHire.IsChecked == true)
            {
                if (datePickerCarFrom.SelectedDate == null || datePickerCarTo.SelectedDate == null
                    || comBoxDriver.SelectionBoxItem.ToString() == String.Empty)
                {
                    MessageBox.Show("Please provide all car hire details.");
                    return;
                }
            }
            if (client == null)
            {
                MessageBox.Show("Please provide client's details.");
                return;
            }

            var facade = BusinessFacadeSingleton.Instance();
            DateTime startDate = datePickerStartDate.SelectedDate.Value.Date;
            DateTime endDate = datePickerEndDate.SelectedDate.Value.Date;
            bool eveningMeal = checkBoxEvMeals.IsChecked.Value;
            bool breakfast = checkBoxBreakfast.IsChecked.Value;
            bool carHire = checkBoxCarHire.IsChecked.Value;
            DateTime hireStart = DateTime.Today;
            DateTime hireEnd = DateTime.Today;
            string driver = comBoxDriver.SelectionBoxItem.ToString();


            int chaletId = Convert.ToInt32(comBoxChaletId.SelectionBoxItem.ToString());
            int bookingRef = DataLayerFacade.GetNextBookingRef();
            if (existingClient != null) // sets the client as an existing client to avoid duplicaiton
            {
                client = existingClient;
            }
            AbstractBooking booking = facade.CreateBooking(bookingRef, startDate, endDate, client, guests, chaletId, eveningMeal,
                breakfast, carHire, hireStart, hireEnd, driver);

            DataLayerFacade.SaveBooking(booking, breakfast, eveningMeal);
            // add the booking to the database
            Reset();
            MessageBox.Show("The booking has been created. The booking reference number is: " + booking.BookingRefNo);
            this.Close();
        }



        // checks if a correct arrival date has been selected
        private void datePickerStartDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime selectedDate = datePickerStartDate.SelectedDate.Value.Date;
            if (selectedDate < DateTime.Today)
            {
                MessageBox.Show("Please enter correct arrival date");
                datePickerStartDate.SelectedDate = DateTime.Today;
            }
        }

        // checks if a correct departure date has been selected
        private void datePickerEndDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime selectedDate = datePickerEndDate.SelectedDate.Value.Date;
            if (selectedDate < DateTime.Today.Add(TimeSpan.FromDays(1)))
            {
                MessageBox.Show("Please enter correct departure date");
                datePickerEndDate.SelectedDate = DateTime.Today.Add(TimeSpan.FromDays(1));
            }
        }

        // shows controls to allow addition of a new booking
        public void ShowFields()
        {
            lblCustDet.Visibility = Visibility.Visible;
            lblCustAddress.Visibility = Visibility.Visible;
            lblGuestDet.Visibility = Visibility.Visible;
            lblGuestName.Visibility = Visibility.Visible;
            lblAge.Visibility = Visibility.Visible;
            lblPassNo.Visibility = Visibility.Visible;
            lblGuestList.Visibility = Visibility.Visible;
            lblClientName.Visibility = Visibility.Visible;
            txtBoxGuestName.Visibility = Visibility.Visible;
            txtBoxGuestAge.Visibility = Visibility.Visible;
            txtBoxGuestPasspNo.Visibility = Visibility.Visible;
            txtBoxCustAddress.Visibility = Visibility.Visible;
            txtBoxCustName.Visibility = Visibility.Visible;
            listBoxGuestList.Visibility = Visibility.Visible;
            checkBoxAddAsGuest.Visibility = Visibility.Visible;
            btnAddGuest.Visibility = Visibility.Visible;
            checkBoxBreakfast.Visibility = Visibility.Visible;
            checkBoxEvMeals.Visibility = Visibility.Visible;
            checkBoxCarHire.Visibility = Visibility.Visible;
            btnConfirmBooking.Visibility = Visibility.Visible;
            lblChooseChalet.Visibility = Visibility.Visible;
            comBoxChaletId.Visibility = Visibility.Visible;
            btnCancel.Visibility = Visibility.Visible;
            checkBoxExistCust.Visibility = Visibility.Visible;
        }

        // when the window is opened controls to make a booking are hidden
        // until availability has been checked and "Book" button has been pressed
        public void HideFields()
        {
            lblCustDet.Visibility = Visibility.Hidden;
            lblCustAddress.Visibility = Visibility.Hidden;
            lblGuestDet.Visibility = Visibility.Hidden;
            lblGuestName.Visibility = Visibility.Hidden;
            lblAge.Visibility = Visibility.Hidden;
            lblPassNo.Visibility = Visibility.Hidden;
            lblGuestList.Visibility = Visibility.Hidden;
            lblClientName.Visibility = Visibility.Hidden;
            txtBoxGuestName.Visibility = Visibility.Hidden;
            txtBoxGuestAge.Visibility = Visibility.Hidden;
            txtBoxGuestPasspNo.Visibility = Visibility.Hidden;
            txtBoxCustAddress.Visibility = Visibility.Hidden;
            txtBoxCustName.Visibility = Visibility.Hidden;
            listBoxGuestList.Visibility = Visibility.Hidden;
            checkBoxAddAsGuest.Visibility = Visibility.Hidden;
            btnAddGuest.Visibility = Visibility.Hidden;
            lblCarHiFrom.Visibility = Visibility.Hidden;
            lblCarHiTo.Visibility = Visibility.Hidden;
            datePickerCarFrom.Visibility = Visibility.Hidden;
            datePickerCarTo.Visibility = Visibility.Hidden;
            checkBoxBreakfast.Visibility = Visibility.Hidden;
            checkBoxEvMeals.Visibility = Visibility.Hidden;
            checkBoxCarHire.Visibility = Visibility.Hidden;
            btnConfirmBooking.Visibility = Visibility.Hidden;
            lblChooseChalet.Visibility = Visibility.Hidden;
            comBoxChaletId.Visibility = Visibility.Hidden;
            btnCancel.Visibility = Visibility.Hidden;
            checkBoxExistCust.Visibility = Visibility.Hidden;
            txtBoxSearchName.Visibility = Visibility.Hidden;
            btnSearch.Visibility = Visibility.Hidden;
            comBoxSearchResult.Visibility = Visibility.Hidden;
            lblDriver.Visibility = Visibility.Hidden;
            comBoxDriver.Visibility = Visibility.Hidden;
            btnDeleteGuest.Visibility = Visibility.Hidden;
        }

        // clears all controls in the window
        public void Reset()
        {
            txtBoxCustName.Text = "";
            txtBoxCustAddress.Text = "";
            txtBoxGuestName.Text = "";
            txtBoxGuestAge.Text = "";
            txtBoxGuestPasspNo.Text = "";
            listBoxGuestList.Items.Clear();
            comBoxChaletId.Items.Clear();
            comBoxDriver.Items.Clear();
            checkBoxAddAsGuest.IsChecked = false;
            checkBoxBreakfast.IsChecked = false;
            checkBoxCarHire.IsChecked = false;
            checkBoxEvMeals.IsChecked = false;
            checkBoxExistCust.IsChecked = false;
            client = null;
            guests.Clear();
            HideFields();

        }

        // closes the window
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // displays controls that allow serching for an existing customers
        private void checkBoxExistCust_Checked(object sender, RoutedEventArgs e)
        {
            txtBoxSearchName.Visibility = Visibility.Visible;
            btnSearch.Visibility = Visibility.Visible;
            comBoxSearchResult.Visibility = Visibility.Visible;
        }

        // hides controls that allow serching for an existing customers
        private void checkBoxExistCust_Unchecked(object sender, RoutedEventArgs e)
        {
            txtBoxSearchName.Visibility = Visibility.Hidden;
            btnSearch.Visibility = Visibility.Hidden;
            comBoxSearchResult.Visibility = Visibility.Hidden;
            txtBoxSearchName.Text = "";
            comBoxSearchResult.Items.Clear();
        }

        // logic for find existing customer button. Returns a list of existing customers in a combo box
        // that match specified name
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            comBoxSearchResult.Items.Clear();
            if (txtBoxSearchName.Text == String.Empty)
            {
                MessageBox.Show("Please proivde a name you wish to search for.");
                return;
            }
            string findCustomer = txtBoxSearchName.Text.ToLower();
            foreach (var customer in DataLayerFacade.FindCustomerByName(findCustomer))
            {
                comBoxSearchResult.Items.Add(customer.Name);
            }
            comBoxSearchResult.IsDropDownOpen = true;
        }

        // populates customer details when an existing customer is selected from combobox
        private void comBoxSearchResult_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comBoxSearchResult.SelectedIndex == -1)
            {
                return;
            }
            string selectedCustomer = Convert.ToString(comBoxSearchResult.SelectedItem);
            CustomerItem customer = DataLayerFacade.GetOneCustomerDetails(selectedCustomer);
            txtBoxCustName.Text = customer.Name;
            txtBoxCustAddress.Text = customer.Address;
        }

        // hides/shows a delete button when a guest is selected from the guest list
        private void listBoxGuestList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBoxGuestList.SelectedIndex == -1)
            {
                btnDeleteGuest.Visibility = Visibility.Hidden;
            }
            else
            {
                btnDeleteGuest.Visibility = Visibility.Visible;
            }
        }

        // deletes a guest from the guest list
        private void btnDeleteGuest_Click(object sender, RoutedEventArgs e)
        {
            string name = listBoxGuestList.SelectedItem.ToString();
            for (int i = 0; i < guests.Count; i++)
            {
                if (guests.ElementAt(i).Name.Equals(name))
                {
                    guests.Remove(guests.ElementAt(i));
                    comBoxDriver.Items.Remove(name);
                    listBoxGuestList.Items.Remove(name);
                    listBoxGuestList.SelectedIndex = -1;
                }
            }
        }

    }
}
