﻿using News_Platform.DTOs;
using News_Platform.Repositories.Interfaces;
using News_Platform.Services.Interfaces;
using News_Platform.Utilities;

namespace News_Platform.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly IUserTokenService _userTokenService;
        private readonly IConfiguration _configuration;

        private readonly JWTUtility _jwtUtility;


        public UserService(IUserRepository userRepository, JWTUtility jwtUtility, IUserTokenService userTokenService, IEmailService emailService, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _jwtUtility = jwtUtility;
            _userTokenService = userTokenService;
            _emailService = emailService;
            _configuration = configuration;
        }


        public async Task<long> RegisterUserAccount(RegisterRequest registerRequest)
        {
            User user = new User
            {
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
                Email = registerRequest.Email,
                PasswordHash = PasswordUtility.HashPassword(registerRequest.Password)
            };

            await _userRepository.AddUserAsync(user);


            var token = await _userTokenService.GenerateTokenAsync(user.UserID, 1);

            string baseUrl = _configuration["AppSettings:FrontendUrl"];

            string activationLink = $"{baseUrl}/activate/{token}";

            Dictionary<string, string> emailParams = new Dictionary<string, string>
            {
                { "[NAME]", user.FirstName + " " + user.LastName },
                { "[ACTIVATION_LINK]", activationLink }
            };


            await _emailService.SendEmailAsync(user.Email, "ACCOUNT_ACTIVATION_1", emailParams);

            return user.UserID;
        }



        public async Task<long> RegisterJournalistUser(JournalistUserCreationDto adminUserCreationDto)
        {
            string generatedPassword = GenerateSecurePassword();

            User user = new User
            {
                FirstName = adminUserCreationDto.FirstName,
                LastName = adminUserCreationDto.LastName,
                Email = adminUserCreationDto.Email,
                Role = 1,
                PasswordHash = PasswordUtility.HashPassword(generatedPassword)
            };


            await _userRepository.AddUserAsync(user);

            var token = await _userTokenService.GenerateTokenAsync(user.UserID, 2);

            string baseUrl = _configuration["AppSettings:FrontendUrl"];
            string activationLink = $"{baseUrl}/activate-journalist/{token}";


            Dictionary<string, string> emailParams = new Dictionary<string, string>
            {
                { "[NAME]", $"{user.FirstName} {user.LastName}" },
                { "[ACTIVATION_LINK]", activationLink },
                { "[TEMPORARY_PASSWORD]", generatedPassword }
            };


            await _emailService.SendEmailAsync(user.Email, "ACCOUNT_ACTIVATION_2", emailParams);

            return user.UserID;
        }


        private string GenerateSecurePassword()
        {
            using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                byte[] data = new byte[16];
                rng.GetBytes(data);
                return Convert.ToBase64String(data).Substring(0, 12);
            }
        }




        public async Task<string?> LoginUser(LoginRequest loginRequest)
        {
            if (loginRequest == null || string.IsNullOrEmpty(loginRequest.Email) || string.IsNullOrEmpty(loginRequest.Password))
            {
                throw new ArgumentException("Invalid login request.");
            }

            User? user = await GetUserByEmailAsync(loginRequest.Email);

            if (user == null || !PasswordUtility.VerifyPassword(loginRequest.Password, user.PasswordHash))
            {
                return null;
            }
            if (user.Status == 0)
            {
                
                throw new UnauthorizedAccessException("User is not activated.");
            }


            return _jwtUtility.GenerateToken(user.UserID, user.Role, user.FirstName + " " + user.LastName, user.Email);
        }

        public async Task ChangeUserPassword(long userId, string oldPassword, string newPassword)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new UnauthorizedAccessException("User not found.");
            }


            if (!PasswordUtility.VerifyPassword(oldPassword, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Old password is incorrect.");
            }

            user.PasswordHash = PasswordUtility.HashPassword(newPassword);
            await _userRepository.UpdateUserAsync(user);
        }

        public async Task ActivateUserAsync(long userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
                throw new InvalidOperationException("User not found.");
            if (user.Status == 1)
                throw new InvalidOperationException("User is already activated.");

            user.Status = 1;
            await _userRepository.UpdateUserAsync(user);
        }

        public async Task ResendActivationEmailAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            if (user.Status != 0)
            {
                throw new InvalidOperationException("User is already activated.");
            }
            if(user.Role == 1)
            {
                throw new InvalidOperationException("Invalid request.");
            }

            // Generate a new activation token
            var token = await _userTokenService.GenerateTokenAsync(user.UserID, 1);
            string baseUrl = _configuration["AppSettings:FrontendUrl"];
            string activationLink = $"{baseUrl}/activate/{token}";

            Dictionary<string, string> emailParams = new Dictionary<string, string>
            {
                { "[NAME]", $"{user.FirstName} {user.LastName}" },
                { "[ACTIVATION_LINK]", activationLink }
            };

            await _emailService.SendEmailAsync(user.Email, "ACCOUNT_ACTIVATION_1", emailParams);
        }


        public async Task<bool> ActivateJournalistAccountAsync(JournalistActivationDto activationDto)
        {
            var userId = await _userTokenService.GetUserIdFromTokenAsync(activationDto.Token, 2);
            if (userId == null)
            {
                throw new InvalidOperationException("Invalid or expired activation token.");
            }

            var user = await _userRepository.GetUserByIdAsync(userId.Value);
            if (user == null || user.Role != 1)
            {
                throw new InvalidOperationException("Invalid journalist account.");
            }

            bool isTempPasswordValid = PasswordUtility.VerifyPassword(activationDto.TemporaryPassword, user.PasswordHash);
            if (!isTempPasswordValid)
            {
                throw new UnauthorizedAccessException("Invalid temporary password.");
            }

            user.PasswordHash = PasswordUtility.HashPassword(activationDto.NewPassword);
            user.Status = 1;

            await _userRepository.UpdateUserAsync(user);

            return true;
        }






        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        public async Task<User?> GetUserByIdAsync(long id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetUserByEmailAsync(email);
        }

        public async Task CreateUserAsync(User user)
        {
            await _userRepository.AddUserAsync(user);
        }

        public async Task UpdateUserAsync(User user)
        {
            await _userRepository.UpdateUserAsync(user);
        }

        public async Task DeleteUserAsync(long id)
        {
            await _userRepository.DeleteUserAsync(id);
        }
    }
}
