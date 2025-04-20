namespace askJiffy_service.Exceptions
{
    public class VehicleNotFoundException : Exception
    {
        //Overloaded Constructors
        public VehicleNotFoundException() : base("Vehicle not found.") { }

        public VehicleNotFoundException(string message) : base(message) { }

        //helps debug nested errors
        public VehicleNotFoundException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
