using Microsoft.AspNetCore.Mvc;
using News_Platform.Services.Interfaces;
using News_Platform.Utilities;
using News_Platform.DTOs;
namespace News_Platform.Controllers
{
    [Route("api/journalists")]
    [ApiController]
    public class JournalistController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserTokenService _userTokenService;

        public JournalistController(IUserService userService, IUserTokenService userTokenService)
        {
            _userService = userService;
            _userTokenService = userTokenService;
        }

        [HttpGet("validate-activation/{token}")]
        public async Task<IActionResult> ValidateActivationToken(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Invalid activation token.");
            }

            var isValid = await _userTokenService.ValidateTokenAsync(token, 2);
            if (!isValid)
            {
                return BadRequest("Invalid or expired activation token.");
            }

            return Ok(new { message = "Activation token is valid." });
        }

        [HttpPost("activate")]
        public async Task<IActionResult> ActivateJournalistAccount([FromBody] JournalistActivationDto activationDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                bool isActivated = await _userService.ActivateJournalistAccountAsync(activationDto);

                if (isActivated)
                {
                    return Ok(new { message = "Journalist account activated successfully." });
                }

                return BadRequest(new { error = "Activation failed due to an unknown reason." });
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
                return StatusCode(500, new { error = "An unexpected error occurred. Please try again later." });
            }
        }




    }

}
