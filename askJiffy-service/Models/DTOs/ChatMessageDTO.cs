using askJiffy_service.Enums;

namespace askJiffy_service.Models.DTOs
{
    public class ChatMessageDTO
    {
        public int Id { get; set; }
        //Foreign key referencing chatSession
        public int ChatSessionId { get; set; }
        public QuestionType QuestionType { get; set; }
        public required string Question { get; set; }
        public required string Response { get; set; }
    }
}
