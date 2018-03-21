using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using BusinessObjects;
using DataLayer;

namespace Presentation
{
    /// <summary>
    /// Piotr Handkowski
    /// Interaction logic for BookingDetails.xaml
    /// Last modified: 09/12/2017
    /// </summary>
    public partial class BookingDetails : Window
    {
        int bookingRef = BookingList.bookingRef; // booking reference taken from a selected item from previous window
        public static GuestDecorator selectedGuest = null; // selected guest from the guest list
        public static AbstractBooking booking = null; // booking from which all details are taken
        public BookingDetails()
        {
            InitializeComponent();
            disableControls();
            txtBoxEditPassp.MaxLength = 10;
            txtBoxEditName.MaxLength = 50;

            // below code retreives booking information from the database and constructs a booking object
            // which is then used to populate booking details in the window
            booking = DataLayerFacade.RetreiveBooking(bookingRef);
            booking.BookingRefNo = bookingRef;
            datePickArrival.SelectedDate = booking.ArrivalDate;
            datePickDepart.SelectedDate = booking.DepartureDate;
            lblBookingRef1.Content = bookingRef;
            lblCustName.Content = booking.Client.Name;
            lblCustAddress.Content = booking.Client.Address;
            foreach (var guest in booking.GuestList)
            {
                listBoxGuests.Items.Add(guest.Name);
                comBoxDriver.Items.Add(guest.Name);
            }
            if (booking.GetType() == typeof(CarHireDecorator))
            {
                checkBoxCarHire.IsChecked = true;
                datePickHireStart.SelectedDate = ((CarHireDecorator)booking).StartDate;
                datePickHireEnd.SelectedDate = ((CarHireDecorator)booking).EndDate;
                comBoxDriver.SelectedItem = ((CarHireDecorator)booking).Driver;
                lblHireFrom.Visibility = Visibility.Visible;
                lblHireTo.Visibility = Visibility.Visible;
                lblDriver.Visibility = Visibility.Visible;
                datePickHireStart.Visibility = Visibility.Visible;
                datePickHireEnd.Visibility = Visibility.Visible;
                comBoxDriver.Visibility = Visibility.Visible;
                var carDecorator = (CarHireDecorator)booking;
                if (carDecorator.Component.GetType() == typeof(BreakfastDecorator))
                {
                    checkBoxBreakfast.IsChecked = true;
                    var breakfasrDecorator = (BreakfastDecorator)carDecorator.Component;
                    if (breakfasrDecorator.Component.GetType() == typeof(EveningMealDecorator))
                    {
                        checkBoxEvMeal.IsChecked = true;
                    }
                }
                if (carDecorator.Component.GetType() == typeof(EveningMealDecorator))
                {
                    checkBoxEvMeal.IsChecked = true;
                }
            }
            if (booking.GetType() == typeof(BreakfastDecorator))
            {
                checkBoxBreakfast.IsChecked = true;
                var breakfasrDecorator = (BreakfastDecorator)booking;
                if (breakfasrDecorator.Component.GetType() == typeof(EveningMealDecorator))
                {
                    checkBoxEvMeal.IsChecked = true;
                }
            }
            if (booking.GetType() == typeof(EveningMealDecorator))
            {
                checkBoxEvMeal.IsChecked = true;
            }

            comBoxChaletId.Items.Add(booking.ChaletId);
            comBoxChaletId.SelectedItem = booking.ChaletId;

        }


        // btn that enables controls so that booking can be changed
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            enableControls();
        }


        // logic for selecting a guest from the guest list. Set the selected guest and enables edit and 
        //delete buttons
        private void listBoxGuests_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBoxGuests.SelectedIndex == -1)
            {
                return;
            }
            foreach (var guest in booking.GuestList)
            {
                if ((string)listBoxGuests.SelectedItem == guest.Name)
                {
                    selectedGuest = (GuestDecorator)guest;
                }
            }
            btnDelGuest.IsEnabled = true;
            btnAmendGuest.IsEnabled = true;
        }

        // logic for Check Availability button. Checks the database if there are any chalets available
        // during the selected period
        private void btnCheckAvail_Click(object sender, RoutedEventArgs e)
        {
            if (datePickArrival.SelectedDate == null)
            {
                MessageBox.Show("Please provide Check In date");
                return;
            }
            if (datePickDepart.SelectedDate == null)
            {
                MessageBox.Show("Please provide Departure date");
                return;
            }
            if (datePickArrival.SelectedDate >= datePickDepart.SelectedDate)
            {
                MessageBox.Show("Please note that departure date must be after arrival date");
                return;
            }
            DateTime startDate = datePickArrival.SelectedDate.Value.Date;
            DateTime endDate = datePickDepart.SelectedDate.Value.Date;
            datePickHireStart.SelectedDate = startDate;
            datePickHireEnd.SelectedDate = endDate;
            datePickHireStart.DisplayDateStart = startDate;
            datePickHireStart.DisplayDateEnd = endDate.Subtract(TimeSpan.FromDays(1));
            datePickHireEnd.DisplayDateStart = startDate.Add(TimeSpan.FromDays(1));
            datePickHireEnd.DisplayDateEnd = endDate;

            datePickHireEnd.DisplayDateEnd = endDate;
            int currentChalet = (int)comBoxChaletId.SelectedItem;
            List<int> availableChalets = DataLayer.DataLayerFacade.AvailableChalets(startDate, endDate, currentChalet);
            if (availableChalets.Count == 0)
            {
                MessageBox.Show("There are no chalets available during this period. Please choose different dates");
                disableControls();
                return;
            }
            else
            {
                comBoxChaletId.Items.Clear();
                foreach (var chalet in availableChalets)
                {
                    comBoxChaletId.Items.Add(chalet);
                    if (chalet == currentChalet)
                    {
                        comBoxChaletId.SelectedItem = currentChalet;
                    }
                }
                MessageBox.Show("There are available chalets in this period.");
                enableControls();
            }
        }

        // logic for delete guest button. Removes the guest from the current object and from the database
        private void btnDelGuest_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxGuests.SelectedIndex == -1)
            {
                return;
            }
            booking.GuestList.Remove(selectedGuest);
            DataLayerFacade.DeleteGuest(selectedGuest.PassportNumber);
            listBoxGuests.SelectedIndex = -1;
            listBoxGuests.Items.Remove(selectedGuest.Name);
            MessageBox.Show("Guest has been deleted.");
            btnDelGuest.IsEnabled = false;
            btnAmendGuest.IsEnabled = false;
            btnAmendGuestSave.Visibility = Visibility.Hidden;
            txtBoxEditAge.Visibility = Visibility.Hidden;
            txtBoxEditName.Visibility = Visibility.Hidden;
            txtBoxEditPassp.Visibility = Visibility.Hidden;
        }

        // logic for amend guest button. Displays text boxes with guest details that can be edited
        private void btnAmendGuest_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxGuests.SelectedIndex == -1)
            {
                return;
            }
            btnAmendGuestSave.Visibility = Visibility.Visible;
            txtBoxEditAge.Visibility = Visibility.Visible;
            txtBoxEditName.Visibility = Visibility.Visible;
            txtBoxEditPassp.Visibility = Visibility.Visible;
            txtBoxEditName.Text = selectedGuest.Name;
            txtBoxEditPassp.Text = selectedGuest.PassportNumber;
            txtBoxEditAge.Text = Convert.ToString(selectedGuest.Age);
        }

        // logic for amend guest save button. Saves the changes in the current object and the database
        private void btnAmengGuestSave_Click(object sender, RoutedEventArgs e)
        {
            btnAmendGuestSave.Visibility = Visibility.Hidden;
            txtBoxEditAge.Visibility = Visibility.Hidden;
            txtBoxEditName.Visibility = Visibility.Hidden;
            txtBoxEditPassp.Visibility = Visibility.Hidden;
            DataLayerFacade.AmendGuest(txtBoxEditName.Text, txtBoxEditPassp.Text,
                Convert.ToInt32(txtBoxEditAge.Text), selectedGuest.PassportNumber);
            if (selectedGuest.Component != null)
            {
                if (selectedGuest.Component.GetType() == typeof(Client))
                {
                    selectedGuest.Component.Name = txtBoxEditName.Text;
                    DataLayerFacade.AmendCustomer(((Client)selectedGuest.Component).CustomerNumber,
                        txtBoxEditName.Text, ((Client)selectedGuest.Component).Address);
                }
            }
            selectedGuest.Name = txtBoxEditName.Text;
            selectedGuest.PassportNumber = txtBoxEditPassp.Text;
            selectedGuest.Age = Convert.ToInt32(txtBoxEditAge.Text);
            txtBoxEditAge.Text = "";
            txtBoxEditName.Text = "";
            txtBoxEditPassp.Text = "";
            listBoxGuests.Items.Clear();
            foreach (var guest in booking.GuestList)
            {
                listBoxGuests.Items.Add(guest.Name);
            }
        }

        // logic for add guest button. Displays boxes where new guest's details can be entered
        private void btnAddGuest_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxGuests.Items.Count >= 6)
            {
                MessageBox.Show("Maximum 6 guests allowed per booking.");
                return;
            }
            lblNewName.Visibility = Visibility.Visible;
            lblNewPass.Visibility = Visibility.Visible;
            lblNewAge.Visibility = Visibility.Visible;
            btnNewGuestConf.Visibility = Visibility.Visible;
            btnAmendGuest.Visibility = Visibility.Hidden;
            btnDelGuest.Visibility = Visibility.Hidden;
            txtBoxEditAge.Visibility = Visibility.Visible;
            txtBoxEditName.Visibility = Visibility.Visible;
            txtBoxEditPassp.Visibility = Visibility.Visible;
            btnCnlNewGuest.Visibility = Visibility.Visible;
        }

        // logic for cancel (new guest) button. Clears and hides boxes where new guest's details are entered
        private void btnCnlNewGuest_Click(object sender, RoutedEventArgs e)
        {
            HideAddNewGuestControls();
            txtBoxEditAge.Text = "";
            txtBoxEditName.Text = "";
            txtBoxEditPassp.Text = "";
        }

        // logic for add new guest button. Adds a new guest to the current booking object and to the database
        private void btnNewGuestConf_Click(object sender, RoutedEventArgs e)
        {
            BusinessFacadeSingleton businessFacase = BusinessFacadeSingleton.Instance();
            var newguest = businessFacase.CreateGuest(txtBoxEditName.Text, txtBoxEditPassp.Text,
                Convert.ToInt32(txtBoxEditAge.Text));
            booking.GuestList.Add(newguest);
            listBoxGuests.Items.Add(newguest.Name);
            DataLayerFacade.AddGuestToBooking(bookingRef, txtBoxEditName.Text,
                txtBoxEditPassp.Text, Convert.ToInt32(txtBoxEditAge.Text));
            txtBoxEditAge.Text = "";
            txtBoxEditName.Text = "";
            txtBoxEditPassp.Text = "";
            HideAddNewGuestControls();
        }

        // logic for the save changes button. Saves other changes to the booking in the current object and 
        // in the database
        private void btnSaveCh_Click(object sender, RoutedEventArgs e)
        {
            DateTime arrivalDate = datePickArrival.SelectedDate.Value.Date;
            DateTime departureDate = datePickDepart.SelectedDate.Value.Date;
            int chaletId = Convert.ToInt32(comBoxChaletId.SelectionBoxItem);
            bool isBreakfastDec = checkBoxBreakfast.IsChecked.Value;
            bool isEveningMealDec = checkBoxEvMeal.IsChecked.Value;
            DataLayerFacade.AmendBooking(bookingRef, arrivalDate, departureDate,
                chaletId, isBreakfastDec, isEveningMealDec);
            bool isCarHireDec = checkBoxCarHire.IsChecked.Value;
            if (isCarHireDec)
            {
                DateTime hireStart = datePickHireStart.SelectedDate.Value.Date;
                DateTime hireEnd = datePickHireEnd.SelectedDate.Value.Date;
                string driver = comBoxDriver.SelectionBoxItem.ToString();
                if (booking.GetType() == typeof(CarHireDecorator))
                {
                    DataLayerFacade.AmendCarHire(driver, bookingRef,
                        hireStart, hireEnd);
                }
                else
                {
                    DataLayerFacade.AddCarHire(driver, bookingRef,
                        hireStart, hireEnd);
                }
            }
            else
            {
                if (booking.GetType() == typeof(CarHireDecorator))
                {
                    DataLayerFacade.RemoveCarHire(bookingRef);
                }
            }
            this.Close();
            MessageBox.Show("The booking has been amended.");
        }

        // logic for invoice button. Opens an invoice window
        private void btnInvoice_Click(object sender, RoutedEventArgs e)
        {
            Invoice Invoice = new Invoice();
            Invoice.Show();
        }

        // logic for delete booking button. Deletes booking from the database and closes the window
        private void btnCancel_Click_1(object sender, RoutedEventArgs e)
        {
            DataLayerFacade.DeleteBooking(bookingRef);
            this.Close();
            MessageBox.Show("The booking has been deleted");
        }

        // logic for car hire check box. Shows car hire details boxes if checked
        private void checkBoxCarHire_Checked(object sender, RoutedEventArgs e)
        {
            datePickHireStart.Visibility = Visibility.Visible;
            datePickHireEnd.Visibility = Visibility.Visible;
            comBoxDriver.Visibility = Visibility.Visible;
            lblHireFrom.Visibility = Visibility.Visible;
            lblHireTo.Visibility = Visibility.Visible;
            lblDriver.Visibility = Visibility.Visible;
        }

        // logic for car hire check box. Hides car hire details boxes if unchecked
        private void checkBoxCarHire_Unchecked(object sender, RoutedEventArgs e)
        {
            datePickHireStart.Visibility = Visibility.Hidden;
            datePickHireEnd.Visibility = Visibility.Hidden;
            comBoxDriver.Visibility = Visibility.Hidden;
            lblHireFrom.Visibility = Visibility.Hidden;
            lblHireTo.Visibility = Visibility.Hidden;
            lblDriver.Visibility = Visibility.Hidden;
        }

        // hides add new guest controsl
        private void HideAddNewGuestControls()
        {
            lblNewName.Visibility = Visibility.Hidden;
            lblNewPass.Visibility = Visibility.Hidden;
            lblNewAge.Visibility = Visibility.Hidden;
            btnNewGuestConf.Visibility = Visibility.Hidden;
            btnAmendGuest.Visibility = Visibility.Visible;
            btnDelGuest.Visibility = Visibility.Visible;
            txtBoxEditAge.Visibility = Visibility.Hidden;
            txtBoxEditName.Visibility = Visibility.Hidden;
            txtBoxEditPassp.Visibility = Visibility.Hidden;
            btnCnlNewGuest.Visibility = Visibility.Hidden;
        }

        // disables all controls in the window so the booking cannot be edited
        public void disableControls()
        {
            datePickArrival.IsEnabled = false;
            datePickDepart.IsEnabled = false;
            btnCheckAvail.Visibility = Visibility.Hidden;
            listBoxGuests.IsEnabled = false;
            checkBoxEvMeal.IsEnabled = false;
            checkBoxBreakfast.IsEnabled = false;
            checkBoxCarHire.IsEnabled = false;
            datePickHireStart.IsEnabled = false;
            datePickHireEnd.IsEnabled = false;
            btnSaveCh.Visibility = Visibility.Hidden;
            comBoxDriver.IsEnabled = false;
            datePickHireStart.Visibility = Visibility.Hidden;
            datePickHireEnd.Visibility = Visibility.Hidden;
            comBoxDriver.Visibility = Visibility.Hidden;
            lblHireFrom.Visibility = Visibility.Hidden;
            lblHireTo.Visibility = Visibility.Hidden;
            lblDriver.Visibility = Visibility.Hidden;
            comBoxChaletId.IsEnabled = false;
            btnAmendGuestSave.Visibility = Visibility.Hidden;
            txtBoxEditAge.Visibility = Visibility.Hidden;
            txtBoxEditName.Visibility = Visibility.Hidden;
            txtBoxEditPassp.Visibility = Visibility.Hidden;
            lblNewName.Visibility = Visibility.Hidden;
            lblNewPass.Visibility = Visibility.Hidden;
            lblNewAge.Visibility = Visibility.Hidden;
            btnNewGuestConf.Visibility = Visibility.Hidden;
            btnAddGuest.IsEnabled = false;
            btnAmendGuest.IsEnabled = false;
            btnDelGuest.IsEnabled = false;
            btnCnlNewGuest.Visibility = Visibility.Hidden;
        }

        // enables controls allowing for the booking to be edited
        public void enableControls()
        {
            datePickArrival.IsEnabled = true;
            datePickDepart.IsEnabled = true;
            listBoxGuests.IsEnabled = true;
            checkBoxEvMeal.IsEnabled = true;
            checkBoxBreakfast.IsEnabled = true;
            checkBoxCarHire.IsEnabled = true;
            datePickHireStart.IsEnabled = true;
            datePickHireEnd.IsEnabled = true;
            btnSaveCh.Visibility = Visibility.Visible;
            comBoxDriver.IsEnabled = true;
            comBoxChaletId.IsEnabled = true;
            btnCheckAvail.Visibility = Visibility.Visible;
            btnSaveCh.Visibility = Visibility.Visible;
            btnAddGuest.IsEnabled = true;
            btnAmendGuest.IsEnabled = true;
            btnDelGuest.IsEnabled = true;
            btnSaveCh.Visibility = Visibility.Visible;
        }



    }
}
