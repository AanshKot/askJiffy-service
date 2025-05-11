using askJiffy_service.Models.DTOs;

namespace askJiffy_service.Utils
{
    public static class Utils
    {
        //or could have it as a static extension method of the VehicleDTO Poco 
        public static string ToVehicleStringContext(this VehicleDTO vehicleDTO) 
        {
            var parts = new List<string>
            {
                vehicleDTO.Year.ToString(),
                vehicleDTO.Make,
                vehicleDTO.Model
            };

            if (!string.IsNullOrWhiteSpace(vehicleDTO.Trim))
                parts.Add(vehicleDTO.Trim);

            if (vehicleDTO.Transmission.HasValue)
                parts.Add(vehicleDTO.Transmission.Value.ToString());

            if (vehicleDTO.Mileage.HasValue)
                parts.Add($"{vehicleDTO.Mileage.Value} km");

            if (!string.IsNullOrWhiteSpace(vehicleDTO.Chassis))
                parts.Add($"Chassis: {vehicleDTO.Chassis}");

            return string.Join(" ", parts);
        }
    }
}
