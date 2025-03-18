using askJiffy_service.Business.DAL;
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

        //could pass trigger == SignUp here then we don't need to check if user is new or not
        public async Task<UserProfile> GetOrCreateUser(ValidateUserRequest validateUserRequest)
        {
            // 1. Need to throw an error if invalid email AND/OR provider
            // 2. Need to verify somehow how user is coming from my clientside app and is authenticated and authorized
            // otherwise we throw an unauthorized exception
            // 3. Call the DAL once those checks are done
            

            var userDTO = await _userDAL.GetOrCreateUser(validateUserRequest.Email,validateUserRequest.Provider);
        }
    }
}
