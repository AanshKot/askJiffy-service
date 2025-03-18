using askJiffy_service.Business.BL;
using askJiffy_service.Models.Requests;
using askJiffy_service.Models.Responses.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace askJiffy_service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserBL _userBL;
        public UserController(ILogger<UserController> logger, IUserBL userBL) { 
            _logger = logger;
            _userBL = userBL;
        }

        //this endpoint verifies user access token and checks if the user exists in the DB, if the user doesn't
        //it creates one
        [Authorize] // forces bearer authentication, request must contain valid access token in header (look at Program.cs addAuthentication)
        [HttpGet("ValidateUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //IAction represents an action method in a controller, used to return various types of HTTP responses 
        //from controller method
        //name might be sessionToken no its not sessionToken in cookies its something else
        public async Task<ActionResult<UserProfile>> ValidateUser([FromBody] ValidateUserRequest validateUserRequest) {
            try
            {
                var validatedUser = await _userBL.GetOrCreateUser(validateUserRequest);
                return Ok(validatedUser);
            }
            catch (UnauthorizedAccessException ue)
            {
                _logger.LogError(ue,"Unauthorized request");
                return Unauthorized();
            }
            catch (ArgumentException ex) {
                _logger.LogError(ex,"Bad Request");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                //dont have to add the ProduceResponse type header because it is implicit behaviour, ASP.NET Core auto returns a 500
                _logger.LogError(ex,"Server Error");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
