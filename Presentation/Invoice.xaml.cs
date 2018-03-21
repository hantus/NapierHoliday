using System;
using System.Windows;
using BusinessObjects;

namespace Presentation
{
    /// <summary>
    /// Piotr Handkowski
    /// Interaction logic for Invoice.xaml
    /// Last modified :09/12/2017
    /// </summary>
    public partial class Invoice : Window
    {
        public AbstractBooking booking = BookingDetails.booking;
        public Invoice()
        {
            // populates the booking's cost details
            InitializeComponent();
            lblCustName.Content = booking.Client.Name;
            lblCustAddress.Content = booking.Client.Address;
            lblArrival.Content = booking.ArrivalDateSt;
            lblDepart.Content = booking.DepartureDateSt;
            lblNight1.Content = "";
            lblNight2.Content = booking.NightCount();
            lblNight3.Content = booking.NightCount();
            lblNight4.Content = booking.NightCount();
            lblNight5.Content = booking.NightCount();
            lblGuestNoBasFee.Content = "";
            lblGuestNoGuestSup.Content = booking.GuestCount();
            lblTotalCarHire.Content = "";
            lblGuestNoBre.Content = "";
            lblTotalBreakf.Content = "";
            lblGuestNoEM.Content = "";
            lblTotalEvMeal.Content = "";
            lblInvoiceTotal.Content = "£" + booking.TotalPriece();

            if (booking.GetType() == typeof(CarHireDecorator))
            {
                var carDecorator = (CarHireDecorator)booking;
                DateTime carHireStart = carDecorator.StartDate;
                DateTime carHireEnd = carDecorator.EndDate;
                int carHiredays = (int)(carHireEnd - carHireStart).Days;
                lblNight1.Content = carHiredays;
                lblTotalCarHire.Content = "£" + carDecorator.CalculateCost();
                if (carDecorator.Component.GetType() == typeof(BreakfastDecorator))
                {
                    var breakfasrDecorator = (BreakfastDecorator)carDecorator.Component;
                    lblGuestNoBre.Content = breakfasrDecorator.GuestCount();
                    lblTotalBreakf.Content = "£" + breakfasrDecorator.CalculateCost();
                    if (breakfasrDecorator.Component.GetType() == typeof(EveningMealDecorator))
                    {
                        var eveningMealDecorator = (EveningMealDecorator)breakfasrDecorator.Component;
                        lblGuestNoEM.Content = eveningMealDecorator.GuestCount();
                        lblTotalEvMeal.Content = "£" + eveningMealDecorator.CalculateCost();
                        Booking booking1 = (Booking)eveningMealDecorator.Component;
                        lblTotalBasFee.Content = "£" + booking1.CalculateCost();
                        lblTotalGuestFee.Content = "£" + booking1.CalculateGuestSupplement();
                    }
                }
                if (carDecorator.Component.GetType() == typeof(EveningMealDecorator))
                {
                    var eveningMealDecorator = (EveningMealDecorator)carDecorator.Component;
                    lblGuestNoEM.Content = eveningMealDecorator.GuestCount();
                    lblTotalEvMeal.Content = "£" + eveningMealDecorator.CalculateCost();
                    Booking booking1 = (Booking)eveningMealDecorator.Component;
                    lblTotalBasFee.Content = "£" + booking1.CalculateCost();
                    lblTotalGuestFee.Content = "£" + booking1.CalculateGuestSupplement();
                }
                if (carDecorator.Component.GetType() == typeof(Booking))
                {
                    Booking booking1 = (Booking)carDecorator.Component;
                    lblTotalBasFee.Content = "£" + booking1.CalculateCost();
                    lblTotalGuestFee.Content = "£" + booking1.CalculateGuestSupplement();
                }
            }
            if (booking.GetType() == typeof(BreakfastDecorator))
            {
                var breakfasrDecorator = (BreakfastDecorator)booking;
                lblGuestNoBre.Content = breakfasrDecorator.GuestCount();
                lblTotalBreakf.Content = "£" + breakfasrDecorator.CalculateCost();
                if (breakfasrDecorator.Component.GetType() == typeof(EveningMealDecorator))
                {
                    var eveningMealDecorator = (EveningMealDecorator)breakfasrDecorator.Component;
                    lblGuestNoEM.Content = eveningMealDecorator.GuestCount();
                    lblTotalEvMeal.Content = "£" + eveningMealDecorator.CalculateCost();
                    Booking booking1 = (Booking)eveningMealDecorator.Component;
                    lblTotalBasFee.Content = "£" + booking1.CalculateCost();
                    lblTotalGuestFee.Content = "£" + booking1.CalculateGuestSupplement();
                }
                if (breakfasrDecorator.Component.GetType() == typeof(Booking))
                {
                    Booking booking1 = (Booking)breakfasrDecorator.Component;
                    lblTotalBasFee.Content = "£" + booking1.CalculateCost();
                    lblTotalGuestFee.Content = "£" + booking1.CalculateGuestSupplement();
                }
            }
            if (booking.GetType() == typeof(EveningMealDecorator))
            {
                var eveningMealDecorator = (EveningMealDecorator)booking;
                lblGuestNoEM.Content = eveningMealDecorator.GuestCount();
                lblTotalEvMeal.Content = "£" + eveningMealDecorator.CalculateCost();
                Booking bookingbase = (Booking)eveningMealDecorator.Component;
                lblTotalBasFee.Content = "£" + bookingbase.CalculateCost();
                lblTotalGuestFee.Content = "£" + bookingbase.CalculateGuestSupplement();
            }
            if (booking.GetType() == typeof(Booking))
            {
                Booking bookingBase = (Booking)booking;
                lblTotalBasFee.Content = "£" + bookingBase.CalculateCost();
                lblTotalGuestFee.Content = "£" + bookingBase.CalculateGuestSupplement();
            }

        }
    }
}
