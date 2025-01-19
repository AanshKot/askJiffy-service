namespace askJiffy_service.Models.DTOs
{
    public class VehicleDTO
    {
        public int Id { get; set; }
        public string? SubjectId { get; set; }
        public string? Make { get; set; }
        public string? Model { get; set; }
        public string? Year { get; set; }
        public string? Transmission { get; set; }
        public int? Mileage { get; set; }
    }
}
