using BCrypt.Net;
namespace News_Platform.Utilities
{
    public static class PasswordUtility
    {
        // Hash a password
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        // Verify a password against a hash
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
