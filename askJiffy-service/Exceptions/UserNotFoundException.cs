namespace askJiffy_service.Exceptions
{
    public class UserNotFoundException: Exception
    {
        //Overloaded Constructors
        public UserNotFoundException() : base("User not found.") { }

        public UserNotFoundException(string message) : base(message) { }

        //helps debug nested errors
        public UserNotFoundException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
