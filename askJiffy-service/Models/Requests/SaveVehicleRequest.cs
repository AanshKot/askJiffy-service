using askJiffy_service.Enums;
using System.ComponentModel.DataAnnotations;

namespace askJiffy_service.Models.Requests
{
    public class SaveVehicleRequest
    {
        //user will be fetched through email in IdToken
        public string? Chassis {  get; set; }
        [Required]
        public required string Make { get; set; }
        [Required]
        public required string Model { get; set; }

        public string? Trim { get; set; }

        [Required]
        public required int Year { get; set; }
        public Transmission? Transmission { get; set; }
        public int? Mileage { get; set; }
    }
}
