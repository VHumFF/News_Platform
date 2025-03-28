
namespace News_Platform.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(long id);
        Task<User?> GetUserByEmailAsync(string email);
        Task CreateUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(long id);
        Task<long> RegisterUserAccount(News_Platform.DTOs.RegisterRequest registerRequest);
        Task<String?> LoginUser(News_Platform.DTOs.LoginRequest loginRequest);
        Task ChangeUserPassword(long userId, string oldPassword, string newPassword);
    }
}
