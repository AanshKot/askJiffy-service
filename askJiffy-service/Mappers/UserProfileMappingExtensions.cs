using askJiffy_service.Models.DTOs;
using askJiffy_service.Models.Responses.User;

//manual mapping: https://antondevtips.com/blog/the-best-way-to-map-objects-in-dotnet-in-2024?utm_source=roadmap-2025&utm_medium=pdf&utm_campaign=roadmap

namespace askJiffy_service.Mappers
{
    public static class UserProfileMappingExtensions
    {
        //'this' keyword extends the UserDTO class essentially 
        public static UserProfile MapToUserProfile(this UserDTO entity) {
            return new UserProfile
            {
                Id = entity.Id,
                ChatHistory = entity.ChatSessions.MapToUserChatHistory(),
                Vehicles = entity.Vehicles.MapToUserVehicleHistory(),
                Email = entity.Email,
                Role = entity.Role
            };
        }
        public static UserChat MapToUserChat(this ChatSessionDTO chatSessionDTO)
        {
            return new UserChat
            {
                Id = chatSessionDTO.Id,
                Title = chatSessionDTO.Title,
                CreatedAt = chatSessionDTO.CreatedAt,
                UpdatedAt = chatSessionDTO.UpdatedAt
            };
        }
        private static List<UserChat> MapToUserChatHistory(this ICollection<ChatSessionDTO> chatSessions) {
            return chatSessions.Select(chatSession => chatSession.MapToUserChat()).ToList(); 
        }

        private static List<Vehicle> MapToUserVehicleHistory(this ICollection<VehicleDTO> vehicleDTOs) {
            return vehicleDTOs.Select(vehicle => vehicle.MapToUserVehicle()).ToList();
        }
    }
}
