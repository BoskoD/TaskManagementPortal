using System.Text.Json.Serialization;

namespace TaskPortalApi.Infrastructure.JWT.AuthRequests
{
    public class ImpersonationRequest
    {
        [JsonPropertyName("username")]
        public string UserName { get; set; }
    }
}
