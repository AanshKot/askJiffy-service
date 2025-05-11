using askJiffy_service.Models.Requests;
using askJiffy_service.Models.Responses.User;

namespace askJiffy_service.Business.BL
{
    public interface IUserBL
    {
        Task<bool> CreateUserProfile(string email, string provider);
        Task<bool> ExistingUserProfile(string email);
        Task<List<Vehicle>> GetVehicles(string email);
        Task<Vehicle> SaveVehicle(SaveVehicleRequest vehicle, string email);
        Task<Vehicle> UpdateVehicle(int vehicleId, SaveVehicleRequest vehicle, string email);
        Task<bool> DeleteVehicle(string email, int vehicleId);
    }
}
