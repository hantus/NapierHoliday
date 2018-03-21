using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using DataLayer;

namespace Presentation
{
    /// <summary>
    /// Piotr Handkowski
    /// Interaction logic for BookingList.xaml
    /// Last modified: 09/12/2017
    /// </summary>
    public partial class BookingList : Window
    {
        public static int bookingRef;
        public BookingList()
        {
            InitializeComponent();
            GridView gridView = new GridView();
            listViewBookings.View = gridView;
            gridView.Columns.Add(new GridViewColumn { Header = "Booking Ref No ", DisplayMemberBinding = new Binding("Id") });
            gridView.Columns.Add(new GridViewColumn { Header = " Customer's name  ", DisplayMemberBinding = new Binding("Name") });
            gridView.Columns.Add(new GridViewColumn { Header = " Arrival Date  ", DisplayMemberBinding = new Binding("Start") });
            gridView.Columns.Add(new GridViewColumn { Header = " Departure Date  ", DisplayMemberBinding = new Binding("End") });

            // adds each booking to the listView
            foreach (var item in DataLayerFacade.GetBasicBookingInfo())
            {
                listViewBookings.Items.Add(item);
            }
        }

        // gets booking ref number of a clicked item and opens a new window that displays full booking details
        private void listViewBookings_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BasicBookingInfo booking = (BasicBookingInfo)listViewBookings.SelectedItem;
            bookingRef = booking.Id;
            BookingDetails BookingDetails = new BookingDetails();
            BookingDetails.Show();
            this.Close();
        }
    }
}
