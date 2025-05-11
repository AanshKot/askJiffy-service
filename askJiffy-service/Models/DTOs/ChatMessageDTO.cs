using askJiffy_service.Enums;

namespace askJiffy_service.Models.DTOs
{
    public class ChatMessageDTO
    {
        public int Id { get; set; }
        //Navigation property for many to one relationship between ChatSession and ChatMessageDTO
        public required ChatSessionDTO ChatSession { get; set; }
        public required string Question { get; set; }
        public string? Response { get; set; }
    }
}
