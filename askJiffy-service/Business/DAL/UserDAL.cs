using askJiffy_service.Enums;
using askJiffy_service.Models.DTOs;
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

        public async Task<UserDTO?> GetOrCreateUser(string email, string provider)
        {
            var user = await _userDAO.getUserByEmail(email);
            
            if (user == null)
            {
                //create default user and insert
                var defaultUserProfile = new UserDTO
                {
                    Email = email,
                    Provider = provider,
                    Role = UserRole.None,
                    Vehicles = new List<VehicleDTO>(), 
                    ChatSessions = new List<ChatSessionDTO>() 
                };
              
                var insertedUser = await _userDAO.InsertNewUser(defaultUserProfile);

                return insertedUser;
            }
            return user;
        }

        public async Task<UserDTO?> GetUserByEmail(string email)
        {
            var user = await _userDAO.getUserByEmail(email);
            return user;
        }

        public async Task<VehicleDTO> SaveVehicle(VehicleDTO vehicle) {
            var savedVehicleDTO = await _userDAO.SaveNewVehicle(vehicle);
            return savedVehicleDTO;
        }
    }
}
