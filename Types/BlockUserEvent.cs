using Newtonsoft.Json;

namespace EscortBookCustomerProfile.Types;

public class BlockUserEvent
{
    [JsonProperty("userId")]
    public string UserId { get; set; }

    [JsonProperty("status")]
    public string Status { get; set; }
}
