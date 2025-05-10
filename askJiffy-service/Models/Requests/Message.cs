using askJiffy_service.Enums;
using System.ComponentModel.DataAnnotations;

namespace askJiffy_service.Models.Requests
{
    public class Message
    {
        public int? chatMessageId { get; set; }
        [Required]
        public required string QuestionText { get; set; }
    }
}
