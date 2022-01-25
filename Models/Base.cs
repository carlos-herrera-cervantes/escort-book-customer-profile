using System;
using Newtonsoft.Json;

namespace EscortBookCustomerProfile.Models
{
    public class Base
    {
        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}