using askJiffy_service.Enums;
using askJiffy_service.Models.DTOs;
using askJiffy_service.Models.Requests;
using askJiffy_service.Repository.DAOs;

namespace askJiffy_service.Business.DAL
{
    public class UserDAL : IUserDAL
    {
        private readonly ILogger<UserDAL> _logger;
        private readonly IUserDAO _userDAO;

        public UserDAL(ILogger<UserDAL> logger,IUserDAO userDAO) {
            _logger = logger;
            _userDAO = userDAO;
        }


        public async Task<bool> InsertNewUser(string email, string provider)
        {
            var defaultUserProfile = new UserDTO
            {
                Email = email,
                Provider = provider,
                Role = UserRole.None,
                Vehicles = new List<VehicleDTO>(),
                ChatSessions = new List<ChatSessionDTO>()
            };

            return await _userDAO.InsertNewUser(defaultUserProfile);
        }
        public async Task<UserDTO?> GetUserByEmail(string email)
        {
            var user = await _userDAO.getUserByEmail(email);
            return user;
        }
        public async Task<List<VehicleDTO>> GetVehicles(string email)
        {
            return await _userDAO.GetUserVehicles(email);
        }
        public async Task<VehicleDTO> SaveVehicle(VehicleDTO vehicle) {
            var savedVehicleDTO = await _userDAO.SaveNewVehicle(vehicle);
            return savedVehicleDTO;
        }
        public async Task<VehicleDTO> UpdateVehicle(VehicleDTO vehicle)
        {
            return await _userDAO.UpdateVehicle(vehicle);
        }
        public async Task<VehicleDTO> GetUserVehicle(string email, int vehicleId)
        {
            return await _userDAO.GetVehicleById(email, vehicleId);
        }
        public async Task<bool> DeleteVehicle(string email, int vehicleId)
        {
            return await _userDAO.DeleteVehicle(email, vehicleId);
        }

        public async Task<ChatSessionDTO> SaveChatSession(ChatSessionDTO chatSessionDTO)
        {
            return await _userDAO.SaveNewChatSession(chatSessionDTO);
        }

        public async Task<ChatSessionDTO> UpdateChatSession(ChatSessionDTO chatSessionDTO)
        {
            return await _userDAO.UpdateExistingChatSession(chatSessionDTO);
        }

        public async Task<ChatSessionDTO> FindChatSession(string email, int chatSessionId)
        {
            return await _userDAO.FindExistingChatSession(email, chatSessionId);
        }

        public async Task<List<ChatSessionDTO>> GetUserChatSessions(string email)
        {
            return await _userDAO.GetUserChatSessions(email);
        }
    }
}
