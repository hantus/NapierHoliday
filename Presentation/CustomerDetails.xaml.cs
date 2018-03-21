using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using DataLayer;
using BusinessObjects;

namespace Presentation
{
    /// <summary>
    /// Piotr Handkowski
    /// Interaction logic for CustomerDetails.xaml
    /// Last modified: 09/12/2017
    /// </summary>
    public partial class CustomerDetails : Window
    {
        string oldPassportNo; // used when updating guest's passport number
        public CustomerDetails()
        {
            InitializeComponent();
            // below code retreives customer's details from database and displays it in the window
            List<Person> clientList = DataLayerFacade.GetCustomer(CustomerL.customerNumber);
            this.Topmost = true;
            Client client = (Client)clientList.ElementAt(0);
            textBoxName.Text = client.Name;
            textBoxAddress.Text = client.Address;
            textBoxPassNo.MaxLength = 10;
            textBoxAddress.MaxLength = 150;
            textBoxName.MaxLength = 50;
            lblCustNumb.Content = client.CustomerNumber;
            List<Person> guestDecorator = DataLayerFacade.GetGuestDecorator(CustomerL.customerNumber, 0);
            if (guestDecorator.Count == 0)
            {
                textBoxPassNo.Visibility = Visibility.Hidden;
                textBoxAge.Visibility = Visibility.Hidden;
                lblPassNo.Visibility = Visibility.Hidden;
                lblAge.Visibility = Visibility.Hidden;
            }
            else
            {
                // if the customer is also a guest, the guest's details are also displayed
                GuestDecorator guest = (GuestDecorator)guestDecorator.ElementAt(0);
                guest.SetComponent(client);
                textBoxPassNo.Text = guest.PassportNumber;
                oldPassportNo = guest.PassportNumber;
                textBoxAge.Text = guest.Age.ToString();
            }
        }


        // logic for update details button. updates the customer details in the database
        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxName.Text == String.Empty || textBoxAddress.Text == String.Empty)
            {
                MessageBox.Show(@"Please provide all details.");
                return;
            }
            string name = textBoxName.Text;
            string address = textBoxAddress.Text;
            int curtNumber = Convert.ToInt32(lblCustNumb.Content);
            DataLayer.DataLayerFacade.AmendCustomer(curtNumber, name, address);
            if (textBoxPassNo.Visibility == Visibility.Visible)
            {
                if (textBoxAge.Text == String.Empty || textBoxPassNo.Text == String.Empty)
                {
                    MessageBox.Show(@"Please provide all details.");
                    return;
                }
                string passportNo = textBoxPassNo.Text;
                int age;
                try
                {
                    age = Convert.ToInt32(textBoxAge.Text);
                }
                catch (Exception)
                {
                    MessageBox.Show("Please enter age as a number.");
                    return;
                }
                if(age < 0 || age > 101)
                {
                    MessageBox.Show("Please provide age between 0 and 101.");
                    return;
                }
                DataLayerFacade.AmendGuest(name, passportNo, age, oldPassportNo);
             
                
            }

            CustomerL CustomerList = new CustomerL();
            CustomerList.Show();
            this.Close();
            MessageBox.Show("Customer details have been updated.");
        }

        // deletes a customer provided that he/she has no outstanding bookings
        private void btnDeleteCust_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int bookings = DataLayer.DataLayerFacade.OutstandingBookings(Convert.ToInt32(lblCustNumb.Content));
                if (bookings == 0)
                {
                    DataLayerFacade.DeleteCustomer(Convert.ToInt32(lblCustNumb.Content));
                    CustomerL CustomerList = new CustomerL();
                    CustomerList.Show();
                    this.Close();
                    MessageBox.Show("Customer has been deleted");
                }
                else
                {
                    MessageBox.Show("The customer has outstanding bookings. Please cancel those bookings if you want to remove the customer");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to delete the customer at the moment. Please try again later.");
            }
        }
    }
}
