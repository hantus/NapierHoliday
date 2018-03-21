using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using DataLayer;

namespace Presentation
{
    /// <summary>
    /// Piotr Handkowski
    /// Interaction logic for CustomerL.xaml
    /// Last modified: 09/12/2017
    /// </summary>
    public partial class CustomerL : Window
    {
        public static int customerNumber; // customer number that will be used in the CustomerDetails windos
        public CustomerL()
        {
            InitializeComponent();

            GridView gridView = new GridView();
            listViewCustomers.View = gridView;
            gridView.Columns.Add(new GridViewColumn { Header = "Customer Ref No ", DisplayMemberBinding = new Binding("Id") });
            gridView.Columns.Add(new GridViewColumn { Header = "Customer's Name  ", DisplayMemberBinding = new Binding("Name") });
            gridView.Columns.Add(new GridViewColumn { Header = "Customer's Address ", DisplayMemberBinding = new Binding("Address") });

            // iterates through all customers and adds them to the lisView
            foreach (var customer in DataLayerFacade.GelAllCustomersInfo())
            {
                listViewCustomers.Items.Add(customer);
            }
        }
        // saves the selected customer number and opens CustomerDetails window
        private void listViewCustomers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listViewCustomers.SelectedIndex == -1)
                return;

            var item = (CustomerItem)listViewCustomers.SelectedItem;
            customerNumber = item.Id;

            CustomerDetails CustomerDetails = new CustomerDetails();
            CustomerDetails.Show();
            this.Close();
        }
    }
}
