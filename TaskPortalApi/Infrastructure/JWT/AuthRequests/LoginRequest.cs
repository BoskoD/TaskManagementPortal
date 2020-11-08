using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TaskPortalApi.Infrastructure.JWT.AuthRequests
{
    public class LoginRequest
    {
        [Required]
        [JsonPropertyName("username")]
        public string UserName { get; set; }

        [Required]
        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}
