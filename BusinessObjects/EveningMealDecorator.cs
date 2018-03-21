namespace BusinessObjects
{
    /// <summary>
    /// Piotr Handkowski
    /// A class for creating evening meal decorator that is used to decorate a booking
    /// Last modified: 09/12/2017
    /// </summary>
    public class EveningMealDecorator : BookingDecorator

    {
        // override for an inherited method. Returns the cost of evening meals for the booking
        public override double CalculateCost()
        {
            return 10 * Component.GuestCount() * Component.NightCount();
        }
    }
}
