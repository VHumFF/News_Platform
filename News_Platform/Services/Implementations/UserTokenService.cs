using Microsoft.EntityFrameworkCore;
using News_Platform.Models;
using News_Platform.Repositories.Interfaces;
using News_Platform.Services.Interfaces;
using System.Security.Cryptography;

namespace News_Platform.Services.Implementations
{
    public class UserTokenService : IUserTokenService
    {
        private readonly IUserTokenRepository _tokenRepository;

        public UserTokenService(IUserTokenRepository tokenRepository)
        {
            _tokenRepository = tokenRepository;
        }

        private string GenerateSecureToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        }

        public async Task<string> GenerateTokenAsync(long userId, long tokenType)
        {
            var token = GenerateSecureToken();
            var userToken = new UserToken
            {
                UserID = userId,
                Token = token,
                TokenType = tokenType,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            };

            await _tokenRepository.SaveTokenAsync(userToken);
            return token;
        }

        public async Task<bool> ValidateTokenAsync(string token, long tokenType)
        {
            var userToken = await _tokenRepository.GetValidTokenAsync(token, tokenType);
            return userToken != null;
        }

        //public async Task InvalidateTokenAsync(long userId, long tokenType)
        //{
        //    var userToken = await _tokenRepository.GetValidTokenAsync(userId, null, tokenType);
        //    if (userToken != null)
        //    {
        //        await _tokenRepository.DeleteTokenAsync(userToken);
        //    }
        //}

        public async Task<long?> GetUserIdFromTokenAsync(string token, long tokenType)
        {
            var userToken = await _tokenRepository.GetValidTokenAsync(token, tokenType);
            return userToken?.UserID; // Return UserID if valid token is found, otherwise null
        }
    }
}
