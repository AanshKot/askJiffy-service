using askJiffy_service.Models.DTOs;

namespace askJiffy_service.Business.DAL
{
    public interface IUserDAL
    {
        Task<UserDTO?> GetOrCreateUser(string email, string provider);
    }
}
