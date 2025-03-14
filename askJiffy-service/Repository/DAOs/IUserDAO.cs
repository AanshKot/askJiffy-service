using askJiffy_service.Models.DTOs;

namespace askJiffy_service.Repository.DAOs
{
    public interface IUserDAO
    {
        Task<UserDTO?> getUserByEmail(string email);
        Task<bool> InsertNewUser(UserDTO userDTO);
    }
}
