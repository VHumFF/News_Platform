using News_Platform.Services.Interfaces;
using News_Platform.Utilities;


namespace News_Platform.Services.Implementations
{
    public class ForgotPasswordService : IForgotPasswordService
    {
        private readonly IEmailService _emailService;
        private readonly IUserService _userService;
        private readonly IUserTokenService _userTokenService;
        private readonly IConfiguration _configuration;

        public ForgotPasswordService(IEmailService emailService, IUserTokenService userTokenService, IUserService userService, IConfiguration configuration)
        {
            _emailService = emailService;
            _userTokenService = userTokenService;
            _userService = userService;
            _configuration = configuration;
        }

        public async Task<bool> SendResetPasswordEmailAsync(string email)
        {
            try
            {
                var user = await _userService.GetUserByEmailAsync(email);
                if (user == null)
                {
                    throw new Exception("User not found with the given email.");
                }

                var existingToken = await _userTokenService.GetUserTokenAsync(user.UserID, 3);

                if (existingToken != null)
                {
                    await _userTokenService.InvalidateTokenAsync(existingToken.Token, 3);
                }


                var token = await _userTokenService.GenerateTokenAsync(user.UserID, 3);
                string baseUrl = _configuration["AppSettings:FrontendUrl"];
                string resetLink = $"{baseUrl}/reset-password/{token}";

                var emailParams = new Dictionary<string, string>
                {
                    { "[NAME]", $"{user.FirstName} {user.LastName}" },
                    { "[RESET_LINK]", resetLink }
                };

                await _emailService.SendEmailAsync(email, "FORGOT_PASSWORD", emailParams);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to send reset password email.", ex);
            }
        }




        public async Task<bool> ResetPasswordAsync(string token, string newPassword)
        {
            try
            {
                var isValid = await _userTokenService.ValidateTokenAsync(token, 3);
                if (!isValid)
                {
                    throw new UnauthorizedAccessException("Invalid or expired token.");
                }

                var userId = await _userTokenService.GetUserIdFromTokenAsync(token, 3);
                if (userId == null)
                {
                    throw new UnauthorizedAccessException("Invalid or expired token.");
                }

                var user = await _userService.GetUserByIdAsync(userId.Value);
                if (user == null)
                {
                    throw new UnauthorizedAccessException("Invalid or expired token.");
                }

                user.PasswordHash = PasswordUtility.HashPassword(newPassword);
                await _userService.UpdateUserAsync(user);

                await _userTokenService.InvalidateTokenAsync(token, 3);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Password reset failed.", ex);
            }
        }


    }
}