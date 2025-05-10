namespace askJiffy_service.Exceptions
{
    public class ChatMessageNotFoundException : Exception
    {
        //Overloaded Constructors
        public ChatMessageNotFoundException() : base("Chat Message not found.") { }

        public ChatMessageNotFoundException(string message) : base(message) { }

        //helps debug nested errors
        public ChatMessageNotFoundException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
