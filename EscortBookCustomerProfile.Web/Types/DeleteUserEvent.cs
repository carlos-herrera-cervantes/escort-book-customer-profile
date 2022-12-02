using Newtonsoft.Json;

namespace EscortBookCustomerProfile.Web.Types;

public class DeleteUserEvent
{
    [JsonProperty("userId")]
    public string UserId { get; set; }

    [JsonProperty("userType")]
    public string UserType { get; set; }

    [JsonProperty("userEmail")]
    public string UserEmail { get; set; }
}
