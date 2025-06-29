using askJiffy_service.Enums;
using System.ComponentModel.DataAnnotations;

namespace askJiffy_service.Models.Requests
{
    public class Message
    {
        public int? Id { get; set; }
        [Required]
        public required string QuestionText { get; set; }
    }
}
