namespace rest_api_veterinaria.AuthModule;

public class JwtModel
{
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public DateTime IssuedAt { get; set; }
    public DateTime Expiration { get; set; }
}

public class JwtResponse
{
    public string Token { get; set; } = "";
    public DateTime Expire { get; set; }
    public string RefreshToken { get; set; } = "";
    public DateTime RefreshExpire { get; set; }
    public string TokenType { get; set; } = "Bearer";
}

