namespace askJiffy_service.Models.Responses.Chat
{
    public class ChatSessionHistory
    {
        public required int Id { get; set; }
        public required string Title { get; set; }
        public required DateTime UpdatedAt { get; set; }
    }
}
