using Microsoft.AspNetCore.Mvc;

using News_Platform.Services;
using News_Platform.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
namespace News_Platform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (loginRequest == null)
            {
                return BadRequest("Invalid request data.");
            }

            String jwt = await _userService.LoginUser(loginRequest);
            if (jwt == null)
            {
                return Unauthorized("Invalid email or password.");
            }

            return Ok(jwt);
        }


        [Authorize]
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterRequest registerRequest)
        {
            if (registerRequest == null || string.IsNullOrWhiteSpace(registerRequest.Email) || string.IsNullOrWhiteSpace(registerRequest.Password))
            {
                return BadRequest("Invalid request data. Email and Password are required.");
            }

            var existingUser = await _userService.GetUserByEmailAsync(registerRequest.Email);
            if (existingUser != null)
            {
                return BadRequest("Email is already registered.");
            }

            try
            {
                var userId = await _userService.RegisterUserAccount(registerRequest);
                return Ok(new { message = "User registered successfully.", userId });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error registering user: {ex.Message}");
                return StatusCode(500, "An error occurred while creating the account.");
            }
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.OldPassword) || string.IsNullOrWhiteSpace(request.NewPassword))
            {
                return BadRequest("Invalid request data.");
            }

            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User ID not found in token.");
                }


                await _userService.ChangeUserPassword(long.Parse(userId), request.OldPassword, request.NewPassword);

                return Ok(new { message = "Password changed successfully." });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error changing password: {ex.Message}");
                return StatusCode(500, "An error occurred while changing the password.");
            }
        }



    }
}
