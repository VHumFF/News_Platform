namespace News_Platform.Services.Interfaces
{
    public interface IUserTokenService
    {
        Task<string> GenerateTokenAsync(long userId, long tokenType);
        Task<bool> ValidateTokenAsync(string token, long tokenType);
        //Task InvalidateTokenAsync(long userId, long tokenType);
        Task<long?> GetUserIdFromTokenAsync(string token, long tokenType);
    }
}
