using askJiffy_service.Business.DAL;
using askJiffy_service.Exceptions;
using askJiffy_service.Mappers;
using askJiffy_service.Models.Requests;
using askJiffy_service.Models.Responses.User;

namespace askJiffy_service.Business.BL
{
    public class UserBL : IUserBL
    {
        private readonly ILogger<UserBL> _logger;
        private readonly IUserDAL _userDAL; 
        public UserBL(ILogger<UserBL> logger,IUserDAL userDAL) {
            _logger = logger;
            _userDAL = userDAL;
        }

        public async Task<UserProfile> GetOrCreateUser(string email, string provider)
        {            
            var userDTO = await _userDAL.GetOrCreateUser(email,provider) ?? throw new UserNotFoundException("Cannot find or create a user.");

            //map to UserProfile return object
            var userProfile = userDTO.MapToUserProfile();

            return userProfile;
        }


        public async Task<List<Vehicle>> GetVehicles(string email)
        {
            var vehicleDTOList = await  _userDAL.GetVehicles(email);
            var vehicles = new List<Vehicle>();

            foreach(var vehicleDTO in vehicleDTOList) {
                vehicles.Add(vehicleDTO.MapToUserVehicle());
            }
            return vehicles;
        }

        // not checking if vehicle exists, saving vehicle if user exists, for that we need a separate check if the user exists
        // refactor logic for GetOrCreateUser, separate the getting and the creating of a user, get user then check if user exists
        public async Task<Vehicle> SaveVehicle(SaveVehicleRequest vehicle, string email)
        {
       
            var user = await _userDAL.GetUserByEmail(email) ?? throw new UserNotFoundException("User not found. Cannot save vehicle without an associated user.");
            var newVehicle = vehicle.MapToNewVehicleDTO(user);
            var vehicleDTO = await _userDAL.SaveVehicle(newVehicle);
            return vehicleDTO.MapToUserVehicle();  
        }

        public async Task<Vehicle> UpdateVehicle(int vehicleId, SaveVehicleRequest vehicle, string email)
        {
            //get user vehicle
            var userVehicle = await _userDAL.GetUserVehicle(email, vehicleId);
            
            //overloaded method, update user vehicleDTO
            var updatedVehicle = vehicle.MapToVehicleDTO(userVehicle);
             
            var savedVehicle = await _userDAL.UpdateVehicle(updatedVehicle);
            return savedVehicle.MapToUserVehicle();
        }


        public async Task<bool> DeleteVehicle(string email, int vehicleId)
        {
            return await _userDAL.DeleteVehicle(email,vehicleId);
        }
    }
}
