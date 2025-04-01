using System.ComponentModel.DataAnnotations;

namespace askJiffy_service.Models.Requests
{
    public class AddVehicle
    {
        public int? UserId { get; set; }
        public string? Chassis {  get; set; }
        [Required]
        public required string Make { get; set; }
        [Required]
        public required string Model { get; set; }
        [Required]
        public required string Year { get; set; }
        public string? Transmission { get; set; }
        public int? Mileage { get; set; }
    }
}
