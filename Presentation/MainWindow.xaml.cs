using System.Windows;

namespace Presentation
{
    /// <summary>
    /// Piotr Handkowski
    /// Interaction logic for MainWindow.xaml
    /// Last modified: 09/12/2017
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // opens Booking window
        private void btnBook_Click(object sender, RoutedEventArgs e)
        {
            BookingWindow BookingWindow = new BookingWindow();
            BookingWindow.Show();
        }

        // opens Booking list window
        private void btnBookinList_Click(object sender, RoutedEventArgs e)
        {
            BookingList BookingList = new BookingList();
            BookingList.Show();
        }

        // opens Customer List window
        private void btnClients_Click(object sender, RoutedEventArgs e)
        {
            CustomerL CustomerList = new CustomerL();
            CustomerList.Show();
        }
    }
}
