using Microsoft.AspNetCore.Mvc;

using News_Platform.Services;
using News_Platform.DTOs;
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

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (loginRequest == null)
            {
                return BadRequest("Invalid request data.");
            }

            var user = await _userService.LoginUser(loginRequest);
            if (user == null)
            {
                return Unauthorized("Invalid email or password.");
            }

            return Ok("Login successful");
        }



        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterRequest registerRequest)
        {
            if (registerRequest == null)
            {
                return BadRequest("Invalid request data.");
            }

            await _userService.RegisterUserAccount(registerRequest);
            return Ok("User registered successfully.");
        }
    }
}
