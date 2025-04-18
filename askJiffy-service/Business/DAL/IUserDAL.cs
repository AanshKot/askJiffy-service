using askJiffy_service.Models.DTOs;

namespace askJiffy_service.Business.DAL
{
    public interface IUserDAL
    {
        Task<UserDTO?> GetOrCreateUser(string email, string provider);
        //returning the saved VehicleDTO as we need the Id of newly saved VehicleDTO
        Task<VehicleDTO> SaveVehicle(VehicleDTO vehicle);
        Task<UserDTO?> GetUserByEmail(string email);
    }
}
