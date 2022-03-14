using Newtonsoft.Json;
using System.Collections.Generic;

namespace EscortBookCustomerProfile.Types
{
    public class DecodedJwt
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("roles")]
        public List<string> Roles { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("iat")]
        public int Iat { get; set; }

        [JsonProperty("exp")]
        public int Exp { get; set; }
    }

    public class Payload
    {
        [JsonProperty("user")]
        public DecodedJwt User { get; set; }
    }
}
