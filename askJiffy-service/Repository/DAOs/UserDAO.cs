using askJiffy_service.Models;
using askJiffy_service.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace askJiffy_service.Repository.DAOs
{
    public class UserDAO : IUserDAO
    {
        private readonly ILogger<UserDAO> _logger;
        private readonly AskJiffyDBContext _context;

        public UserDAO(ILogger<UserDAO> logger, AskJiffyDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        //if simply using Task<UserDTO> the editor will complain as there is a possibility of null return
        //add a ? to indicate the return can be null
        public async Task<UserDTO?> getUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(user => user.Email.Equals(email));
        }

        public async Task<bool> InsertNewUser(UserDTO userDTO)
        {
            try {
                _context.Users.Add(userDTO);
                await _context.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error inserting user: {Email}", userDTO.Email);
                return false;
            }
            
        }
    }
}
