using askJiffy_service.Models.DTOs;
using askJiffy_service.Models.Requests;

namespace askJiffy_service.Business.DAL
{
    public interface IUserDAL
    {
        Task<UserDTO?> GetOrCreateUser(string email, string provider);
        Task<UserDTO?> GetUserByEmail(string email);
        Task<List<VehicleDTO>> GetVehicles(string email);
        //returning the saved VehicleDTO as we need the Id of newly saved VehicleDTO
        Task<VehicleDTO> SaveVehicle(VehicleDTO vehicle);
        Task<VehicleDTO> UpdateVehicle(VehicleDTO vehicle);
        Task<VehicleDTO> GetUserVehicle(string email, int vehicleId);
        Task<bool> DeleteVehicle(string email, int vehicleId);
        Task<ChatSessionDTO> SaveChatSession(ChatSessionDTO chatSessionDTO);
        Task<ChatSessionDTO> UpdateChatSession(ChatSessionDTO chatSessionDTO);
        Task<ChatSessionDTO> FindChatSession(string email, int chatSessionId);
        Task<List<ChatSessionDTO>> GetUserChatSessions(string email);
    }
}
