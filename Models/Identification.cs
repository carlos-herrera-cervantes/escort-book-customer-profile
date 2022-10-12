using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace EscortBookCustomerProfile.Models;

[Table("identification", Schema = "public")]
public class Identification : Base
{
    #region snippet_Properties

    [Column("id")]
    public string ID { get; set; } = Guid.NewGuid().ToString();

    [Column("path")]
    public string Path { get; set; }

    [Column("customer_id")]
    public string CustomerID { get; set; }

    [Column("identification_part_id")]
    public string IdentificationPartID { get; set; }

    #endregion
}

public class CreateIdentificationDto
{
    #region snippet_Properties

    [Required]
    [JsonProperty("image")]
    public IFormFile Image { get; set; }

    [Required]
    [JsonProperty("identificationPartId")]
    public string IdentificationPartID { get; set; }

    #endregion

    #region snippet_Deconstructors

    public void Deconstruct(out IFormFile image, out string identificationPartID)
        => (image, identificationPartID) = (Image, IdentificationPartID);

    #endregion
}
