using askJiffy_service.Models.Requests;
using askJiffy_service.Models.Responses.User;

namespace askJiffy_service.Business.BL
{
    public interface IUserBL
    {
        Task<UserProfile> GetOrCreateUser(string email, string provider);
    }
}
