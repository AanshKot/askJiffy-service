namespace askJiffy_service.Exceptions
{
    public class ChatSessionNotFoundException : Exception
    {
        //Overloaded Constructors
        public ChatSessionNotFoundException() : base("Chat Session not found.") { }

        public ChatSessionNotFoundException(string message) : base(message) { }

        //helps debug nested errors
        public ChatSessionNotFoundException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
