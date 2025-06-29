using askJiffy_service.Business.DAL;
using askJiffy_service.Exceptions;
using askJiffy_service.Mappers;
using askJiffy_service.Models.DTOs;
using askJiffy_service.Models.Requests;
using askJiffy_service.Models.Responses.Chat;
using askJiffy_service.Services;
using askJiffy_service.Utils;
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
            var user = await _userDAL.GetUserByEmail(email) ?? throw new UserNotFoundException("User not found. Cannot create new Chat without an associated user.");

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

            newChatSessionDTO.PushChatMessage(chatRequest.InitialQuestionText);

            var createdChatSessionDTO = await _userDAL.SaveChatSession(newChatSessionDTO);

            return createdChatSessionDTO.MapToUserChatSession();
        }

        //builds response and flushes after each chunk is read
        public async Task StreamResponseAsync(string email, int chatSessionId, Message question, HttpResponse response)
        {
            // https://stackoverflow.com/questions/52098863/whats-the-difference-between-text-event-stream-and-application-streamjson
            response.ContentType = "text/event-stream";
            var fullResponse = new StringBuilder();

            var chatSessionDTO = await _userDAL.FindChatSession(email, chatSessionId);

            var vehicleStringContext = chatSessionDTO.Vehicle.ToVehicleStringContext();

            // the response from gemini service's streamAnswerAsync is a IAsyncEnumerable<string> to forward this lazily returned iterable can use a foreach loop
            // examples: https://learn.microsoft.com/en-us/archive/msdn-magazine/2019/november/csharp-iterating-with-async-enumerables-in-csharp-8
            await foreach (var textChunk in _geminiService.StreamAnswerAsync(vehicleStringContext, question.QuestionText)) {

                fullResponse.Append(textChunk);

                var buffer = Encoding.UTF8.GetBytes(textChunk);

                // https://learn.microsoft.com/en-us/dotnet/api/system.io.stream.writeasync?view=net-9.0
                // arguments: byte translated text, offset, length of buffer
                await response.Body.WriteAsync(buffer, 0, buffer.Length);

                // asynchronously sends all currently buffered output to the client
                await response.Body.FlushAsync();
            }

            // if question comes in with an Id that means the user is either editing a prexisting message or wants the answer to the initial question
            // save new response to chatMessage
            if (question.Id != null)
            {
                var chatMessage = chatSessionDTO.ChatMessages.FirstOrDefault(cm => cm.Id == question.Id) ?? throw new ChatMessageNotFoundException("Unable to find chat message");
                chatMessage.Question = question.QuestionText; //for edited messages
                chatMessage.Response = fullResponse.ToString();
            }
            else
            { 
                chatSessionDTO.PushChatMessage(question.QuestionText,fullResponse.ToString());
            }


            chatSessionDTO.UpdatedAt = DateTime.UtcNow;
            await _userDAL.UpdateChatSession(chatSessionDTO);
        }

        public async Task<List<ChatSessionHistory>> GetChatSessions(string email)
        {
            var chatSessionDTOs = await _userDAL.GetUserChatSessions(email);

            var chatSessionHistoryList = new List<ChatSessionHistory>();

            foreach (var chatSessionDTO in chatSessionDTOs)
            {
                chatSessionHistoryList.Add(chatSessionDTO.MapToUserChatHistorySession());
            }

            return chatSessionHistoryList;
        }

        public async Task<ChatSession> GetChatSession(string email, int chatSessionId)
        {
            var chatSessionDTO = await _userDAL.FindChatSession(email, chatSessionId);

            return chatSessionDTO.MapToUserChatSession();
        }
    }
}
