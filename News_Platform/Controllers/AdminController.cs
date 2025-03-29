using Microsoft.AspNetCore.Mvc;
using News_Platform.DTOs;
using News_Platform.Services.Implementations;
using News_Platform.Services.Interfaces;
namespace News_Platform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : Controller
    {
        private readonly IUserService _userService;

        public AdminController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpPost("register-journalist")]
        public async Task<IActionResult> RegisterJournalistUser([FromBody] JournalistUserCreationDto adminUserCreationDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingUser = await _userService.GetUserByEmailAsync(adminUserCreationDto.Email);
            if (existingUser != null)
            {
                return BadRequest("Email is already registered.");
            }

            try
            {
                var userId = await _userService.RegisterJournalistUser(adminUserCreationDto);
                return Ok(new { message = "User registered successfully.", userId });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error registering user: {ex.Message}");
                return StatusCode(500, "An error occurred while creating the account.");
            }
        }
    }
}
