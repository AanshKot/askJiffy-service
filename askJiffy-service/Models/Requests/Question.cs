using askJiffy_service.Enums;
using System.ComponentModel.DataAnnotations;

namespace askJiffy_service.Models.Requests
{
    public class Question
    {
        [Required]
        public required string QuestionText { get; set; }
        [Required]
        public required QuestionType QuestionType { get; set; }

    }
}
