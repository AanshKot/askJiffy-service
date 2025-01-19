using askJiffy_service.Enums;
using System.ComponentModel.DataAnnotations;

namespace askJiffy_service.Models.Requests
{
    public class ChatRequest
    {
        [Required]
        public required string SubjectId { get; set; }
        [Required]
        public int VehicleId { get; set; }
        [Required]
        public required string InitialQuestionText { get; set; }
        [Required]
        public questionType QuestionType { get; set; }
    }
}
