using System.Text.Json.Serialization;

namespace TaskPortalApi.Infrastructure.JWT.AuthRequests
{
    public class RefreshTokenRequest
    {
        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; }
    }
}
