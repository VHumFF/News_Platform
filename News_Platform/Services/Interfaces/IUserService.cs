using News_Platform.DTOs;

namespace News_Platform.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(long id);
        Task<User?> GetUserByEmailAsync(string email);
        Task CreateUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(long id);
        Task<long> RegisterUserAccount(DTOs.RegisterRequest registerRequest);
        Task<long> RegisterJournalistUser(DTOs.AdminUserCreationDto adminUserCreationDto);
        Task<string?> LoginUser(DTOs.LoginRequest loginRequest);
        Task ChangeUserPassword(long userId, string oldPassword, string newPassword);
        Task ActivateUserAsync(long userId);
        Task ResendActivationEmailAsync(string email);
        Task<bool> ActivateJournalistAccountAsync(JournalistActivationDto activationDto);
    }
}
