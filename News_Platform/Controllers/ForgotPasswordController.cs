using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using News_Platform.Services.Interfaces;
using News_Platform.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace News_Platform.Controllers
{
    public class ForgotPasswordController : Controller
    {
        private readonly IForgotPasswordService _forgotPasswordService;

        public ForgotPasswordController(IForgotPasswordService forgotPasswordService)
        {
            _forgotPasswordService = forgotPasswordService;
        }

        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email))
            {
                return BadRequest("Email is required.");
            }

            try
            {
                var result = await _forgotPasswordService.SendResetPasswordEmailAsync(request.Email);
                if (!result)
                {
                    return NotFound("User not found with the given email.");
                }

                return Ok("Password reset email sent.");
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Token) || string.IsNullOrWhiteSpace(request.NewPassword))
            {
                return BadRequest("Token and new password are required.");
            }

            try
            {
                await _forgotPasswordService.ResetPasswordAsync(request.Token, request.NewPassword);
                return Ok("Password reset successful.");
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
