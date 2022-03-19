using System.Text.Json.Serialization;

namespace AlfonsBot.Models;

public class AuthenticateResponse
{
    [JsonPropertyName("jwtToken")]
    public string Token { get; set; }
    [JsonPropertyName("refreshToken")]
    public string RefreshToken { get; set; }
}
