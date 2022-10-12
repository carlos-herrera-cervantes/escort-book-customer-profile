using System;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace EscortBookCustomerProfile.Models;

public class Base
{
    [Column("created_at")]
    [JsonProperty("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    [JsonProperty("updatedAt")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
