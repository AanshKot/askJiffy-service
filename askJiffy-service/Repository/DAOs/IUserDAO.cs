using askJiffy_service.Models.DTOs;

namespace askJiffy_service.Repository.DAOs
{
    public interface IUserDAO
    {
        Task<UserDTO?> getUserByEmail(string email);
        Task<bool> InsertNewUser(UserDTO userDTO);
        Task<List<VehicleDTO>> GetUserVehicles(string email);
        Task<VehicleDTO> SaveNewVehicle(VehicleDTO vehicleDTO);
        Task<VehicleDTO> UpdateVehicle(VehicleDTO vehicleDTO);
        Task<VehicleDTO> GetVehicleById(string email, int vehicleId);
        Task<bool> DeleteVehicle(string email, int vehicleId);
        Task<ChatSessionDTO> SaveNewChatSession(ChatSessionDTO chatSessionDTO);
        Task<ChatSessionDTO> UpdateExistingChatSession(ChatSessionDTO chatSessionDTO);
        Task<ChatSessionDTO> FindExistingChatSession(string email, int chatSessionId);
        Task<List<ChatSessionDTO>> GetUserChatSessions(string email);
    }
}
