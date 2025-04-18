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
            /* chose to proceed with explicit loading, this is because eager loading fetches data at the same time as the main entity
             eager loading basically joins the User, Chat and Vehicle tables then queries for matching user email this query can be 
            expensive if the email doesn't exist, joining tables for no reason. With explicit loading you can fetch data on demand,
            after the main entity is loaded. Since the related data isn't always needed (i.e. when the user doesn't exist)

            can drill down and include multiple levels of related data using the .ThenInclude method */
            var user = await _context.Users.FirstOrDefaultAsync(user => user.Email.Equals(email));

            if (user != null) { 
                await _context.Entry(user).Collection(user => user.ChatSessions).LoadAsync();
                await _context.Entry(user).Collection(user => user.Vehicles).LoadAsync();
            }

            return user;
        }

        public async Task<UserDTO> InsertNewUser(UserDTO userDTO)
        {
                _context.Users.Add(userDTO);
                await _context.SaveChangesAsync();
                return userDTO;       
        }

        public async Task<VehicleDTO> SaveNewVehicle(VehicleDTO vehicleDTO)
        {
            _context.Vehicles.Add(vehicleDTO);
            await _context.SaveChangesAsync();

            /*returned the same object reference as passed in the argument but EFCore updates object in-place when calling saveChangesAsync()
            means vehicleDTO will now have autocreated Id */
            return vehicleDTO;
        }
    }
}
