using Microsoft.AspNetCore.Mvc;
using News_Platform.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using News_Platform.Services.Interfaces;
using News_Platform.Services.Implementations;
namespace News_Platform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserTokenService _userTokenService;

        public AuthController(IUserService userService, IUserTokenService userTokenService)
        {
            _userService = userService;
            _userTokenService = userTokenService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (loginRequest == null)
            {
                return BadRequest("Invalid request data.");
            }

            try
            {
                string? jwt = await _userService.LoginUser(loginRequest);
                if (jwt == null)
                {
                    return Unauthorized("Invalid email or password.");
                }

                return Ok(new { Token = jwt });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
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

        [HttpPost("activate/{token}")]
        public async Task<IActionResult> ActivateAccount(string token)
        {
            try
            {
                string decodedToken = Uri.UnescapeDataString(token);
                var isValid = await _userTokenService.ValidateTokenAsync(decodedToken, 1);
                if (!isValid)
                    return BadRequest("Invalid or expired token.");

                var userId = await _userTokenService.GetUserIdFromTokenAsync(decodedToken, 1);
                if (userId == null)
                    return BadRequest("Invalid token.");

                await _userService.ActivateUserAsync(userId.Value);

                return Ok("Account activated successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while activating the account.");
            }
        }


        [HttpPost("auth/resend-activation")]
        public async Task<IActionResult> ResendActivationEmail([FromBody] ResendActivationRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Email))
            {
                return BadRequest("Email is required.");
            }

            try
            {
                await _userService.ResendActivationEmailAsync(request.Email);
                return Ok("Activation email resent successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while resending the activation email.");
            }
        }








    }
}
