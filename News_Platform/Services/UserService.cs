using News_Platform.DTOs;
using News_Platform.Repositories;
using News_Platform.Utilities;

namespace News_Platform.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        private readonly JWTUtility _jwtUtility;


        public UserService(IUserRepository userRepository, JWTUtility jwtUtility)
        {
            _userRepository = userRepository;
            _jwtUtility = jwtUtility;
        }


        public async Task RegisterUserAccount(RegisterRequest registerRequest)
        {
            User user = new User
            {
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
                Email = registerRequest.Email,
                PasswordHash = PasswordUtility.HashPassword(registerRequest.Password)
            };

            await CreateUserAsync(user);
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

            return _jwtUtility.GenerateToken(user.UserID, user.Role);
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
