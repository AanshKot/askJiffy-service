using askJiffy_service.Models.Requests;
using askJiffy_service.Models.Responses.User;

namespace askJiffy_service.Business.BL
{
    public interface IUserBL
    {
        Task<UserProfile> GetOrCreateUser(string email, string provider);
        Task<Vehicle> SaveVehicle(SaveVehicleRequest vehicle, string email);
        Task<bool> DeleteVehicle(string email, int vehicleId);
    }
}
