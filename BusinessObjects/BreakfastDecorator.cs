namespace BusinessObjects
{

    /// <summary>
    /// Piotr Handkowski
    /// Class used to create breakfast decorator that is used to decorate a booking 
    /// Last modified: 09/12/2017
    /// </summary>
    public class BreakfastDecorator : BookingDecorator
    {
        // override of an inherited method. Returns the cost of the decorator(breakfast)
        public override double CalculateCost()
        {
            return 5 * Component.GuestCount() * Component.NightCount();
        }
    }
}
