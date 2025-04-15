using News_Platform.Models;

namespace News_Platform.Services.Interfaces
{
    public interface IUserTokenService
    {
        Task<string> GenerateTokenAsync(long userId, long tokenType);
        Task<bool> ValidateTokenAsync(string token, long tokenType);
        Task InvalidateTokenAsync(string token, long tokenType);
        Task<long?> GetUserIdFromTokenAsync(string token, long tokenType);
        Task<UserToken> GetUserTokenAsync(long userId, long tokenType);
    }
}
