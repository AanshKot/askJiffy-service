using askJiffy_service.Enums;

namespace askJiffy_service.Models.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public required string SubjectId { get; set; }
        public required string Email { get; set; }
        public required string ProfilePictureUrl { get; set; }
        public required string Provider { get; set; }
        public required UserRole Role { get; set; }
    }
}
