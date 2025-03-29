using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        [HttpPost("register-journalist")]
        public async Task<IActionResult> RegisterJournalistUser([FromBody] JournalistUserCreationDto adminUserCreationDto)
        {
            try
            {
                if (!long.TryParse(User.FindFirst("role")?.Value, out long role) || role != 2)
                {
                    return Forbid("Only Admins can register journalists.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var existingUser = await _userService.GetUserByEmailAsync(adminUserCreationDto.Email);
                if (existingUser != null)
                {
                    return Conflict(new { message = "Email is already registered." });
                }

                var userId = await _userService.RegisterJournalistUser(adminUserCreationDto);
                return Ok(new { message = "Journalist registered successfully.", userId });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An unexpected error occurred while creating the account." });
            }
        }

    }
}
