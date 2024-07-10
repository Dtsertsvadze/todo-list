namespace Services;
using System.IdentityModel.Tokens.Jwt;

public static class DecodeJwtTokenUserId
{
    public static Guid GetUserIdFromToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

        if (jsonToken == null)
        {
            throw new ArgumentException("Invalid JWT token.");
        }

        var userIdClaim = jsonToken.Claims.FirstOrDefault(claim => claim.Type == "userId");
        if (userIdClaim == null)
        {
            throw new ArgumentException("UserId claim not found in the JWT token.");
        }

        if (Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return userId;
        }
        else
        {
            throw new ArgumentException("Invalid userId claim in the JWT token.");
        }
    }

    public static string GetUserNameFromToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

        if (jsonToken == null)
        {
            throw new ArgumentException("Invalid JWT token.");
        }

        var userNameClaim = jsonToken.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.UniqueName);
        if (userNameClaim == null)
        {
            throw new ArgumentException("UserName claim not found in the JWT token.");
        }

        return userNameClaim.Value;
    }
}
