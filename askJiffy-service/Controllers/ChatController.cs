using askJiffy_service.Business.BL;
using askJiffy_service.Exceptions;
using askJiffy_service.Models.Requests;
using askJiffy_service.Models.Responses.Chat;
using askJiffy_service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace askJiffy_service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatController : Controller
    {
        private readonly ILogger<ChatController> _logger;
        private readonly IChatBL _chatBL;
        private readonly IGeminiService _geminiService;

        public ChatController(ILogger<ChatController> logger, IChatBL chatBl, IGeminiService geminiService) {
            _logger = logger;
            _chatBL = chatBl;
            _geminiService = geminiService;
        }

        [Authorize] // forces bearer authentication, request must contain valid ID token in header (look at Program.cs addAuthentication)
        [HttpPost("new")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ChatSession>> StartNewChat([FromBody] ChatRequest chatRequest ) 
        {
            var userEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            var provider = User.FindFirst("iss")?.Value; 

            if (string.IsNullOrEmpty(userEmail) || string.IsNullOrEmpty(provider))
            {
                return BadRequest(new { Error = "Required claims (email and provider) are missing." });
            }

            try
            {
                var newChatSession = await _chatBL.NewChat(userEmail, chatRequest);
                return Ok(newChatSession);
            }
            catch(VehicleNotFoundException vex)
            {
                return BadRequest(vex.Message);
            }
            catch (ApplicationException aex) 
            {
                _logger.LogError(aex, "Error with Gemini call");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error with GeminiService: {aex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Server Error");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        //for existing chat
        [Authorize]
        [HttpPost("{chatSessionId}/message")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task StreamAnswer(int chatSessionId, [FromBody] Message question)
        {
            var userEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            var provider = User.FindFirst("iss")?.Value;
            
            var response = Response;

            if (string.IsNullOrEmpty(userEmail) || string.IsNullOrEmpty(provider))
            {
                //in a streaming response scenario can't use return Unauthorized or throw new Unauthorized because not returning an IAction result
                response.StatusCode = StatusCodes.Status401Unauthorized;
                await response.WriteAsync("Unauthorized: Missing required claims.");
                return;
            }

            await _chatBL.StreamResponseAsync(userEmail, chatSessionId, question, response);
        }

        [Authorize]
        [HttpGet("getchatsessions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<ChatSessionHistory>>> getChatSessions()
        {
            var userEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            var provider = User.FindFirst("iss")?.Value;

            if (string.IsNullOrEmpty(userEmail) || string.IsNullOrEmpty(provider))
            {
                return BadRequest(new { Error = "Required claims (email and provider) are missing." });
            }

            try 
            {
                var chatSessionHistoryList = await _chatBL.GetChatSessions(userEmail);
                return Ok(chatSessionHistoryList);
            }
            catch (UserNotFoundException userEx)
            {
                _logger.LogError(userEx, "User not found");
                return StatusCode(StatusCodes.Status400BadRequest, $"Failed to find User Profile: {userEx.Message}");
            }
            catch (UnauthorizedAccessException ue)
            {
                _logger.LogError(ue, "Unauthorized request");
                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Server Error");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize]
        [HttpGet("{chatSessionId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ChatSession>> getChatSession(int chatSessionId) {
            var userEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            var provider = User.FindFirst("iss")?.Value;

            if (string.IsNullOrEmpty(userEmail) || string.IsNullOrEmpty(provider))
            {
                return BadRequest(new { Error = "Required claims (email and provider) are missing." });
            }

            try
            {
                var chatSession = await _chatBL.GetChatSession(userEmail, chatSessionId);
                return Ok(chatSession);
            }
            catch (UserNotFoundException userEx)
            {
                _logger.LogError(userEx, "User not found");
                return StatusCode(StatusCodes.Status400BadRequest, $"Failed to find User Profile: {userEx.Message}");
            }
            catch (UnauthorizedAccessException ue)
            {
                _logger.LogError(ue, "Unauthorized request");
                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Server Error");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

    }
}
