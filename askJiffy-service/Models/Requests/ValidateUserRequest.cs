using System.ComponentModel.DataAnnotations;

namespace askJiffy_service.Models.Requests
{
    public class ValidateUserRequest
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string Provider { get; set; }
    }
}
