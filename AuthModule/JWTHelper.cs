using csc_srv_carpeta_digital.Exceptions;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace rest_api_veterinaria.AuthModule;

public class JWTHelper
{
    private IConfiguration _configuration;
    public JWTHelper(IConfiguration configuration)
    {
        _configuration = configuration; 
    }

    public JwtResponse GenerateJwtToken(string user)
    {

        int tokenExpirationMinutes = Int32.Parse(_configuration["Jwt:ExpireMin"]);
        DateTime tokenExpiration = DateTime.UtcNow.AddMinutes(tokenExpirationMinutes); //parametrizable

        int refreshTokenExpirationMinutes = Int32.Parse(_configuration["Jwt:ExpireRefreshMin"]);
        DateTime refreshTokenExpiration = DateTime.UtcNow.AddMinutes(refreshTokenExpirationMinutes);

        var claims = new[]
        {
            new Claim("user", user),

        };

        var refreshClaims = new[]
        {
            new Claim("user", user),
            new Claim("isRefreshToken", "true"),
        };

        string signKey = _configuration["Jwt:Key"];
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        string issuer = _configuration["Jwt:Issuer"];
        string audience = _configuration["Jwt:Audience"];

        var token = new JwtSecurityToken(
            claims: claims,
            expires: tokenExpiration,
            signingCredentials: credentials,
            issuer: issuer,
            audience: audience
        );

        var refreshToken = new JwtSecurityToken(
            claims: refreshClaims,
            expires: refreshTokenExpiration,
            signingCredentials: credentials,
            issuer: issuer,
            audience: audience
        );

        return new JwtResponse
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            RefreshToken = new JwtSecurityTokenHandler().WriteToken(refreshToken),
            Expire = tokenExpiration,
            RefreshExpire = refreshTokenExpiration
        };
    }


    public static bool ValidateTokenData(IEnumerable<Claim>? tokenClaims)
    {
        string? issuer = tokenClaims?.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Iss)?.Value;
        string? audience = tokenClaims?.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Aud)?.Value;
        string? expiration = tokenClaims?.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp)?.Value;

        if (issuer == null || audience == null || expiration == null)
        {
            return false;
        }

        try
        {
            long expirationUnix = long.Parse(expiration);
            DateTime expirationDate = DateTimeOffset.FromUnixTimeSeconds(expirationUnix).DateTime;

            // Additional validation on the timestamps, if necessary
            if (expirationDate <= DateTime.UtcNow )
            {
                return false;
            }
        }
        catch
        {
            return false;
        }

        return true;
    }



    public static JwtModel GetTokenData(IEnumerable<Claim> tokenClaims)
    {
        JwtModel jwt = new JwtModel()
        {
            Issuer = GetIssuer(tokenClaims),
            Audience = GetAudience(tokenClaims),
            IssuedAt = GetIssuedAt(tokenClaims),
            Expiration = GetExpiration(tokenClaims)
        };

        return jwt;
    }

    public  string GetUser(IEnumerable<Claim> userClaims)
    {
        string? userId = userClaims.FirstOrDefault(c => c.Type == "user")?.Value;
        if (userId == null)
        {
            throw new AuthException("Invalid JWT");
        }
        return userId;
    }



    /// <summary>
    /// Reads the issuer from the JWT Token
    /// </summary>
    /// <param name="tokenClaims">The list of token claims from the JWT</param>
    /// <returns>The issuer of the JWT</returns>
    private static string GetIssuer(IEnumerable<Claim> tokenClaims)
    {
        string? issuer = tokenClaims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Iss)?.Value;
        if (issuer == null)
        {
            throw new AuthException("Invalid JWT: Missing issuer.");
        }
        return issuer;
    }

    /// <summary>
    /// Reads the audience from the JWT Token
    /// </summary>
    /// <param name="tokenClaims">The list of token claims from the JWT</param>
    /// <returns>The audience of the JWT</returns>
    private static string GetAudience(IEnumerable<Claim> tokenClaims)
    {
        string? audience = tokenClaims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Aud)?.Value;
        if (audience == null)
        {
            throw new AuthException("Invalid JWT: Missing audience.");
        }
        return audience;
    }

    /// <summary>
    /// Reads the issued-at time from the JWT Token
    /// </summary>
    /// <param name="tokenClaims">The list of token claims from the JWT</param>
    /// <returns>The issued-at time of the JWT as a DateTime</returns>
    private static DateTime GetIssuedAt(IEnumerable<Claim> tokenClaims)
    {
        string? issuedAt = tokenClaims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Iat)?.Value;
        if (issuedAt == null)
        {
            throw new AuthException("Invalid JWT: Missing issued-at time.");
        }

        return DateTimeOffset.FromUnixTimeSeconds(long.Parse(issuedAt)).DateTime;
    }

    /// <summary>
    /// Reads the expiration time from the JWT Token
    /// </summary>
    /// <param name="tokenClaims">The list of token claims from the JWT</param>
    /// <returns>The expiration time of the JWT as a DateTime</returns>
    private static DateTime GetExpiration(IEnumerable<Claim> tokenClaims)
    {
        string? expiration = tokenClaims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp)?.Value;
        if (expiration == null)
        {
            throw new AuthException("Invalid JWT: Missing expiration time.");
        }

        return DateTimeOffset.FromUnixTimeSeconds(long.Parse(expiration)).DateTime;
    }
}
