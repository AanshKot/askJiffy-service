using askJiffy_service.Enums;

namespace askJiffy_service.Models.Responses.User
{
    public class Vehicle
    {
        public required int Id { get; set; }
        public required string Make { get; set; }
        public required string Model { get; set; }
        public string? Trim { get; set; }
        public required int Year { get; set; }
        public string? Chassis { get; set; }
        public Transmission? Transmission { get; set; }
        public int? Mileage { get; set; }

        //public string? ImageUrl { get; set; }
    }
}
