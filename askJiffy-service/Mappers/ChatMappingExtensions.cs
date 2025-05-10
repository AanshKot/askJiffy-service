using askJiffy_service.Models.DTOs;
using askJiffy_service.Models.Responses.Chat;

namespace askJiffy_service.Mappers
{
    // used for both chatSession and chatMessage
    public static class ChatMappingExtensions
    {
        public static ChatSession MapToUserChatSession(this ChatSessionDTO chatSessionDTO)
        {
            return new ChatSession
            {
                Id = chatSessionDTO.Id,
                Title = chatSessionDTO.Title,
                CreatedAt = chatSessionDTO.CreatedAt,
                UpdatedAt = chatSessionDTO.UpdatedAt,
                Vehicle = chatSessionDTO.Vehicle.MapToUserVehicle(),
                ChatMessages = chatSessionDTO.ChatMessages.Select(chatMessageDTO => chatMessageDTO.MapToUserChatMessage()).ToList(),
            };
        }

        public static ChatMessage MapToUserChatMessage(this ChatMessageDTO message)
        {
            return new ChatMessage
            {
                Id = message.Id,
                Question = message.Question,
                Response = message.Response,
            };
        }

        public static void PushChatMessage(this ChatSessionDTO chatSessionDTO, string messageContent, string? response = null) {
            var chatMessage = new ChatMessageDTO
            {
                ChatSession = chatSessionDTO,
                Question = messageContent,
                Response = response
            };

            chatSessionDTO.ChatMessages.Add(chatMessage);
        }
    }
}
