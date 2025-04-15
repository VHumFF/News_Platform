using News_Platform.Data;
using News_Platform.Models;
using News_Platform.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace News_Platform.Repositories.Implementations
{
    public class UserTokenRepository : IUserTokenRepository
    {
        private readonly AppDbContext _context;

        public UserTokenRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UserToken> GetUserTokenAsync(long userId, long tokenType)
        {
            return await _context.UserTokens
                .FirstOrDefaultAsync(t => t.UserID == userId && t.TokenType == tokenType);
        }

        public async Task<UserToken> GetValidTokenAsync(string token, long tokenType)
        {
            var validToken = await _context.UserTokens
                .FirstOrDefaultAsync(t => t.Token == token &&
                                          t.TokenType == tokenType &&
                                          t.ExpiresAt > DateTime.UtcNow.AddHours(8));

            Console.WriteLine($"Checking token: {token}");
            Console.WriteLine($"Found token: {(validToken != null ? "Yes" : "No")}");
            Console.WriteLine($"DB Expiration: {validToken?.ExpiresAt}");
            Console.WriteLine($"Current Time: {DateTime.UtcNow.AddHours(8)}");

            return validToken;
        }


        public async Task SaveTokenAsync(UserToken userToken)
        {
            _context.UserTokens.Add(userToken);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTokenAsync(UserToken userToken)
        {
            _context.UserTokens.Remove(userToken);
            await _context.SaveChangesAsync();
        }
    }
}
