using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

namespace InflationDataServer.Tools
{
    public class JwtManager
    {
        private readonly IConfiguration _configuration;

        public JwtManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool ValidateJwtToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            //string secretKey = _configuration.GetValue<string>("JwtConfig:SecretKey");
            string secretKey = Environment.GetEnvironmentVariable("SECRET_KEY") ?? _configuration.GetValue<string>("JwtConfig:SecretKey");
            string audience = Environment.GetEnvironmentVariable("AUDIENCE") ?? _configuration.GetValue<string>("JwtConfig:Audience");
            string issuer = Environment.GetEnvironmentVariable("ISSUER") ?? _configuration.GetValue<string>("JwtConfig:Issuer");

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
            };

            try
            {
                tokenHandler.ValidateToken(token, validationParameters, out _);
                return true; // Token is valid
            }
            catch
            {
                return false; // Token is invalid
            }
        }


        public string GenerateJwtToken(string username)
        {
            int expiryMinutes = _configuration.GetValue<int>("JwtConfig:ExpiryMinutes");
            string secretKey = Environment.GetEnvironmentVariable("SECRET_KEY") ?? _configuration.GetValue<string>("JwtConfig:SecretKey");
            string audience = Environment.GetEnvironmentVariable("AUDIENCE") ?? _configuration.GetValue<string>("JwtConfig:Audience");
            string issuer = Environment.GetEnvironmentVariable("ISSUER") ?? _configuration.GetValue<string>("JwtConfig:Issuer");
            string subject = Environment.GetEnvironmentVariable("SUBJECT") ?? _configuration.GetValue<string>("JwtConfig:Subject");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, subject),
                new Claim(JwtRegisteredClaimNames.UniqueName, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                signingCredentials: signingCredentials
            );

            var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);

            return encodedToken;
        }

        public string GetUsernameInToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            string secretKey = Environment.GetEnvironmentVariable("SECRET_KEY") ?? _configuration.GetValue<string>("JwtConfig:SecretKey");
            //string secretKey = _configuration.GetValue<string>("JwtConfig:SecretKey");


            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                //ValidIssuer = _configuration.GetValue<string>("JwtConfig:Issuer"),
                //ValidAudience = _configuration.GetValue<string>("JwtConfig:Audience"),
                ValidIssuer = Environment.GetEnvironmentVariable("ISSUER") ?? _configuration.GetValue<string>("JwtConfig:Issuer"),
                ValidAudience = Environment.GetEnvironmentVariable("AUDIENCE") ?? _configuration.GetValue<string>("JwtConfig:Audience"),

            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
            };

            // Validate and decode the token
            var claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
            var jwtToken = (JwtSecurityToken)validatedToken;

            // Retrieve the username claim from the token
            var usernameClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.UniqueName);

            return usernameClaim?.Value;
        }
    }
}
