using askJiffy_service.Models.Responses.User;

namespace askJiffy_service.Models.Responses.Chat
{
    public class ChatSession
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required Vehicle Vehicle { get; set; }
        public required ICollection<ChatMessage> ChatMessages {  get; set; }
        public required DateTime CreatedAt { get; set; }
        public required DateTime UpdatedAt { get; set; }
    }
}
