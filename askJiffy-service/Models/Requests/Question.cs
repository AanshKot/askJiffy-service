using askJiffy_service.Enums;
using System.ComponentModel.DataAnnotations;

namespace askJiffy_service.Models.Requests
{
    public class Question
    {
        [Required]
        public required int ChatSessionId { get; set; }
        [Required]
        public required string QuestionText { get; set; }
    }
}
