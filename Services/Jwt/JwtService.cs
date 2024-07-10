namespace Services.Jwt;
using System.Globalization;
using System.Security.Claims;
using Entities.Identity;
using Microsoft.Extensions.Configuration;
using ServiceContracts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

public class JwtService(IConfiguration configuration)
    : IJwtService
{
    public AuthenticationResponse CreateJwtToken(ApplicationUser user)
    {
        DateTime expiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(configuration["Jwt:ExpirationMinutes"]));

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"] !);

        var claims = new List<Claim>
        {
            new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new (JwtRegisteredClaimNames.Sub, user.Email !),
            new (JwtRegisteredClaimNames.Email, user.Email !),
            new (JwtRegisteredClaimNames.UniqueName, user.PersonName !),
            new ("userId", user.Id.ToString()),
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expiration,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return new AuthenticationResponse
        {
            Token = tokenHandler.WriteToken(token),
            Email = user.Email,
            PersonName = user.PersonName,
            Expiration = expiration,
            RefreshToken = GenerateRefreshToken(),
            RefreshTokenExpiration = DateTime.UtcNow.AddMinutes(Convert.ToInt32(configuration["RefreshToken:ExpirationMinutes"])),
        };
    }

    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
    {
        TokenValidationParameters tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidAudience = configuration["Jwt:Audience"],
            ValidateIssuer = true,
            ValidIssuer = configuration["Jwt:Issuer"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"] !)),
            ValidateLifetime = false,
        };

        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        ClaimsPrincipal principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);

        if (validatedToken is not JwtSecurityToken jwtSecurityToken ||
            jwtSecurityToken.Header.Alg.Equals(
                SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }

    private static string GenerateRefreshToken()
    {
        byte[] bytes = new byte[32];

        var randomNumberGenerator = RandomNumberGenerator.Create();

        randomNumberGenerator.GetBytes(bytes);
        return Convert.ToBase64String(bytes);
    }
}
