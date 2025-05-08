using askJiffy_service.Business.DAL;
using askJiffy_service.Exceptions;
using askJiffy_service.Mappers;
using askJiffy_service.Models.DTOs;
using askJiffy_service.Models.Requests;
using askJiffy_service.Models.Responses.Chat;
using askJiffy_service.Services;
using System.Text;

namespace askJiffy_service.Business.BL
{
    public class ChatBL : IChatBL
    {
        private readonly ILogger<ChatBL> _logger;
        private readonly IGeminiService _geminiService;
        private readonly IUserDAL _userDAL;
        public ChatBL(ILogger<ChatBL> logger, IGeminiService geminiService, IUserDAL userDAL) 
        { 
            _logger = logger;
            _geminiService = geminiService;
            _userDAL = userDAL;
        }

        public async Task<ChatSession> NewChat(string email, ChatRequest chatRequest)
        {
            // 1. Find VehicleDTO: Find the User's vehicle that matches the vehicleId and is not soft deleted
            // 2. Use the VehicleDTO and initialQuestion attribute of the chatSession to generate a title for the chatSession
            // 3. Map the vehicle, user, chatSessionDTO to new chatSessionDTO
            // 4. map the initial question to a new chatMessageDTO, push it to the ChatMessages of the newly created chatSessionDTO
            // 5. Call the userDAL to save the newly mapped ChatSessionDTO

            var vehicle = await _userDAL.GetUserVehicle(email, chatRequest.VehicleId);
            var user = await _userDAL.GetUserByEmail(email) ?? throw new UserNotFoundException("User not found. Cannot save vehicle without an associated user.");

            var chatSessionTitle = await _geminiService.GenerateTitleAsync($"List one title (the best fitting one) for a user question: \"${chatRequest.InitialQuestionText}\" centered around this vehicle ${vehicle.Make} ${vehicle.Model} - ${vehicle.Year}");

            var newChatSessionDTO = new ChatSessionDTO
            {
                Title = chatSessionTitle,
                User = user,
                Vehicle = vehicle,
                ChatMessages = new List<ChatMessageDTO>(),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };

            newChatSessionDTO.PushInitialChatMessage(chatRequest.InitialQuestionText);

            var createdChatSessionDTO = await _userDAL.SaveChatSession(newChatSessionDTO);

            return createdChatSessionDTO.MapToUserChatSession();
        }

        //builds response and flushes after each chunk is read
        public async Task StreamResponseAsync(Question question, HttpResponse response)
        {
            response.ContentType = "text/plain";
            var fullResponse = new StringBuilder();

            // the response from gemini service's streamAnswerAsync is a IAsyncEnumerable<string> to forward this lazily returned iterable can use a foreach loop
            // examples: https://learn.microsoft.com/en-us/archive/msdn-magazine/2019/november/csharp-iterating-with-async-enumerables-in-csharp-8
            await foreach (var textChunk in _geminiService.StreamAnswerAsync(question.QuestionText)) {

                fullResponse.Append(textChunk);

                var buffer = Encoding.UTF8.GetBytes(textChunk);

                // https://learn.microsoft.com/en-us/dotnet/api/system.io.stream.writeasync?view=net-9.0
                // arguments: byte translated text, offset, length of buffer
                await response.Body.WriteAsync(buffer, 0, buffer.Length);

                // asynchronously sends all currently buffered output to the client
                await response.Body.FlushAsync();
            }
            
            //have some logic below for saving the message and its completed response to the chatSession list, updating the lastUpdated portion of the chatSession
           
            
        }
    }
}
