using News_Platform.Models;

namespace News_Platform.Repositories.Interfaces
{
    public interface IUserTokenRepository
    {
        Task<UserToken> GetValidTokenAsync(string token, long tokenType);
        Task SaveTokenAsync(UserToken userToken);
        Task DeleteTokenAsync(UserToken userToken);
    }
}
