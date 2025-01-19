namespace askJiffy_service.Models.Requests
{
    public class Vehicle
    {
        public string? SubjectId { get; set; }
        public required string Make { get; set; }
        public required string Model { get; set; }
        public required string Year { get; set; }
        public string? Transmission { get; set; }
        public int? Mileage { get; set; }
    }
}
