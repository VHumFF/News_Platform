using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;


namespace News_Platform.Utilities
{
    public class JWTUtility
    {
        private readonly IConfiguration _configuration;

        public JWTUtility(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(long userId, long role, string name, string email)
        {
            var keyString = _configuration.GetValue<string>("JwtSettings:Key");
            if (string.IsNullOrEmpty(keyString))
                throw new InvalidOperationException("JWT Key is missing from configuration.");

            var key = Encoding.UTF8.GetBytes(keyString);

            var expirationMinutes = _configuration.GetValue<int?>("JwtSettings:ExpirationMinutes") ?? 30;

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim("role", role.ToString()),
                new Claim("username", name),
                new Claim("email", email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8).AddMinutes(expirationMinutes),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}
