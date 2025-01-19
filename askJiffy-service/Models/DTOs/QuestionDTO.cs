using askJiffy_service.Enums;

namespace askJiffy_service.Models.DTOs
{
    public class QuestionDTO
    {
        public int Id { get; set; }
        public required string QuestionText { get; set; }
        public required questionType QuestionType { get; set; }
    }
}
