namespace askJiffy_service.Models.Responses.User
{
    public class UserChat
    {
        public required int Id { get; set; }
        public required string Title { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required DateTime UpdatedAt { get; set; }
    }
}
