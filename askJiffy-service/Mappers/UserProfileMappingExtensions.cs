using askJiffy_service.Models.DTOs;
using askJiffy_service.Models.Responses.User;

namespace askJiffy_service.Mappers
{
    public static class UserProfileMappingExtensions
    {
        public static UserProfile MapToUserProfile(this UserDTO entity) {
            return new UserProfile
            {
                Id = entity.Id,
                ChatHistory = entity.ChatSessions.MapToUserChatHistory(),



            };
        }

        public static List<UserChat> MapToUserChatHistory(this List<ChatSessionDTO> chatSessions) {
            return chatSessions.Select(chatSessions => new UserChat { 
                Id = chatSessions.Id,
                Title = chatSessions.Title,
                CreatedAt = chatSessions.CreatedAt,
                UpdatedAt = chatSessions.UpdatedAt
            }).ToList(); 
        }
    }
}
