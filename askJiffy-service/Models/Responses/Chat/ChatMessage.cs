

namespace askJiffy_service.Models.Responses.Chat
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public required string Question { get; set; }
        public string? Response { get; set; }
    }
}
