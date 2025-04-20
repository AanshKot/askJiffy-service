using askJiffy_service.Enums;

namespace askJiffy_service.Models.Responses.User
{
    public class UserProfile
    {
        public required int Id { get; set; }
        public required string Email { get; set; }
        public required UserRole Role { get; set; }
        public required ICollection<UserChat> ChatHistory { get; set; }
        public required ICollection<Vehicle> Vehicles { get; set; }
    }
}
