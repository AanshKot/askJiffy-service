using askJiffy_service.Enums;

namespace askJiffy_service.Models.DTOs
{
    public class VehicleDTO
    {
        public int Id { get; set; }
        public required UserDTO User { get; set; }
        public string? Chassis { get; set; }
        public required string Make { get; set; }
        public required string Model { get; set; }
        public string? Trim { get; set; }
        public required int Year { get; set; }
        public Transmission? Transmission { get; set; }
        public int? Mileage { get; set; }
        public ICollection<ChatSessionDTO>? ChatSessions { get; set; }
        //soft delete
        public bool IsDeleted { get; set; }
        // public string? ImageUrl {  get; set; }
    }
}
