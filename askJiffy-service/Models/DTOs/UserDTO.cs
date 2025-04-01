using askJiffy_service.Enums;

namespace askJiffy_service.Models.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public required string Email { get; set; }
        public required string Provider { get; set; }
        public required UserRole Role { get; set; }
        public required ICollection<VehicleDTO> Vehicles { get; set; }
        public required ICollection<ChatSessionDTO> ChatSessions { get; set; }
    }
}
