﻿using askJiffy_service.Exceptions;
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
                _context.Users.Add(userDTO);
                await _context.SaveChangesAsync();
                return true;       
        }
        public async Task<List<VehicleDTO>> GetUserVehicles(string email) 
        {
            var user = await _context.Users.Include(u => u.Vehicles).FirstOrDefaultAsync(user => user.Email.Equals(email)) 
            ?? throw new UserNotFoundException("Error Retrieving list of user vehicles: User Profile not Found In DB.");

            var vehicleList = user.Vehicles.Where(v => !v.IsDeleted).ToList();

            return vehicleList;
        }
        public async Task<VehicleDTO> SaveNewVehicle(VehicleDTO vehicleDTO)
        {
            _context.Vehicles.Add(vehicleDTO);
            await _context.SaveChangesAsync();

            /*returned the same object reference as passed in the argument but EFCore updates object in-place when calling saveChangesAsync()
            means vehicleDTO will now have autocreated Id */
            return vehicleDTO;
        }
        public async Task<VehicleDTO> UpdateVehicle(VehicleDTO vehicleDTO)
        {
            _context.Vehicles.Update(vehicleDTO);
            await _context.SaveChangesAsync();
            return vehicleDTO;
        }
        public async Task<VehicleDTO> GetVehicleById(string email, int vehicleId)
        {
            var user = await _context.Users.Include(u => u.Vehicles).FirstOrDefaultAsync(user => user.Email.Equals(email))
             ?? throw new UserNotFoundException("Error Finding Vehicle: User Profile not Found.");

            var vehicle = user.Vehicles.FirstOrDefault(v => v.Id == vehicleId && !v.IsDeleted) 
                ?? throw new VehicleNotFoundException("User vehicle doesn't exist or was deleted");

            return vehicle;
        }

        public async Task<bool> DeleteVehicle(string email, int vehicleId)
        {
            var user = await _context.Users.Include(u => u.Vehicles).FirstOrDefaultAsync(user => user.Email.Equals(email)) 
            ?? throw new UserNotFoundException("Error Deleting Vehicle: User Profile not Found.");

            var vehicle = user.Vehicles.FirstOrDefault(v => v.Id == vehicleId && !v.IsDeleted) 
            ?? throw new VehicleNotFoundException("User has either already deleted this vehicle or this vehicle doesn't exist");

            vehicle.IsDeleted = true;
            await _context.SaveChangesAsync();
            return vehicle.IsDeleted;
        }

        public async Task<ChatSessionDTO> SaveNewChatSession(ChatSessionDTO chatSessionDTO)
        {
            _context.ChatSessions.Add(chatSessionDTO);
            await _context.SaveChangesAsync();
            return chatSessionDTO;
        }

        public async Task<ChatSessionDTO> UpdateExistingChatSession(ChatSessionDTO chatSessionDTO)
        {
            _context.ChatSessions.Update(chatSessionDTO);
            await _context.SaveChangesAsync();
            return chatSessionDTO;
        }

        public async Task<ChatSessionDTO> FindExistingChatSession(string email, int chatSessionId)
        {
            //eager load chatSession and related child nav properties: https://stackoverflow.com/questions/68447966/including-multiple-children-of-child-table
            var user = await _context.Users
                .Include(u => u.ChatSessions)
                    .ThenInclude(cs => cs.ChatMessages)
                .Include(u => u.ChatSessions)
                    .ThenInclude(cs => cs.Vehicle)
                .FirstOrDefaultAsync(user => user.Email.Equals(email))
            ?? throw new UserNotFoundException("Error Finding Chat Session: User Profile not Found.");

            var chatSession = user.ChatSessions.FirstOrDefault(cs => cs.Id == chatSessionId && !cs.IsDeleted) 
            ?? throw new ChatSessionNotFoundException("User chat session doesn't exist or was deleted");

            return chatSession;
        }

        public async Task<List<ChatSessionDTO>> GetUserChatSessions(string email)
        {
            var user = await _context.Users.Include(u => u.ChatSessions).FirstOrDefaultAsync(user => user.Email.Equals(email))
            ?? throw new UserNotFoundException("Error Retrieving list of user chat sessions: User Profile not Found In DB.");

            var chatSessionHistory = user.ChatSessions.Where(cs => !cs.IsDeleted).ToList();

            return chatSessionHistory;
        }
    }
}
