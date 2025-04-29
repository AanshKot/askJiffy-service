using askJiffy_service.Business.BL;
using askJiffy_service.Exceptions;
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
        [Authorize] // forces bearer authentication, request must contain valid ID token in header (look at Program.cs addAuthentication)
        [HttpGet("getprofile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //IAction represents an action method in a controller, used to return various types of HTTP responses 
        //from controller method
        public async Task<ActionResult<UserProfile>> ValidateUser() {
            var userEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            var provider = User.FindFirst("iss")?.Value; //issuer of token "https://accounts.google.com"

            if (string.IsNullOrEmpty(userEmail) || string.IsNullOrEmpty(provider))
            {
                return BadRequest(new { Error = "Required claims (email and provider) are missing." });
            }

            try
            {
                var validatedUser = await _userBL.GetOrCreateUser(userEmail,provider);
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

        [Authorize]
        [HttpGet("getvehicles")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<Vehicle>>> GetVehicles() {
            var userEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            var provider = User.FindFirst("iss")?.Value;

            if (string.IsNullOrEmpty(userEmail) || string.IsNullOrEmpty(provider))
            {
                return BadRequest(new { Error = "Required claims (email and provider) are missing." });
            }

            try
            {
                var vehicleList = await _userBL.GetVehicles(userEmail);
                return Ok(vehicleList);
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
        [HttpPost("savevehicle")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Vehicle>> SaveVehicle([FromBody] SaveVehicleRequest vehicle ) {
            var userEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            var provider = User.FindFirst("iss")?.Value; 

            if (string.IsNullOrEmpty(userEmail) || string.IsNullOrEmpty(provider))
            {
                return BadRequest(new { Error = "Required claims (email and provider) are missing." });
            }

            try
            {
                var savedVehicle = await _userBL.SaveVehicle(vehicle, userEmail);
                return Ok(savedVehicle);
            }
            catch (UserNotFoundException)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Failed to find User Profile");
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Server Error");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize]
        [HttpPut("updatevehicle")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Vehicle>> UpdateVehicle([FromQuery] int vehicleId, [FromBody] SaveVehicleRequest vehicle) {
            var userEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            var provider = User.FindFirst("iss")?.Value;

            if (string.IsNullOrEmpty(userEmail) || string.IsNullOrEmpty(provider))
            {
                return BadRequest(new { Error = "Required claims (email and provider) are missing." });
            }
            try
            {
                var updatedVehicle = await _userBL.UpdateVehicle(vehicleId, vehicle, userEmail);
                return Ok(updatedVehicle);
            }
            catch (UserNotFoundException)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Failed to find User Profile");
            }
            catch (VehicleNotFoundException)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Failed to find Vehicle");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Server Error");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("deletevehicle")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> DeleteVehicle([FromQuery] int vehicleId) {
            var userEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            var provider = User.FindFirst("iss")?.Value;

            if (string.IsNullOrEmpty(userEmail) || string.IsNullOrEmpty(provider))
            {
                return BadRequest(new { Error = "Required claims (email and provider) are missing." });
            }

            try
            {
                var deletedVehicle = await _userBL.DeleteVehicle(userEmail, vehicleId);
                return Ok(deletedVehicle);
            }
            catch (UserNotFoundException)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Failed to find User");
            }
            catch(VehicleNotFoundException) {
                return StatusCode(StatusCodes.Status400BadRequest, $"Failed to find Vehicle with corresponding Id: {vehicleId}");
            }
        }
   
    }
}
