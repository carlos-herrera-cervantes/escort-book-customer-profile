using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace EscortBookCustomerProfile.Models
{
    public class Identification : Base
    {
        #region snippet_Properties

        public string ID { get; set; } = Guid.NewGuid().ToString();

        public string Path { get; set; }

        public string ProfileID { get; set; }

        public string IdentificationPartID { get; set; }

        #endregion

        #region snippet_ForeignProperties

        public Profile Profile { get; set; }

        public IdentificationPart IdentificationPart { get; set; }

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
}