using askJiffy_service.Models.Requests;
using askJiffy_service.Models.Responses.Chat;

namespace askJiffy_service.Business.BL
{
    public interface IChatBL
    {   
        // Returns a newly created ChatSession thread
        Task<ChatSession> NewChat(string email, ChatRequest chatRequest);

        // Returns a streamed response to a question, saves response to new chat message
        Task StreamResponseAsync(string email, int chatSessionId, Message prompt, HttpResponse response);
    }
}
