namespace askJiffy_service.Models.Responses.User
{
    public class UserVehicle
    {
        public required int Id { get; set; }
        public required string Make { get; set; }
        public required string Model { get; set; }
        public required int Year { get; set; }
    }
}
