using askJiffy_service.Models.DTOs;

namespace askJiffy_service.Repository.DAOs
{
    public interface IUserDAO
    {
        Task<UserDTO?> getUserByEmail(string email);
        Task<UserDTO> InsertNewUser(UserDTO userDTO);

        Task<VehicleDTO> SaveNewVehicle(VehicleDTO vehicleDTO);
        Task<bool> DeleteVehicle(string email, int vehicleId);
    }
}
