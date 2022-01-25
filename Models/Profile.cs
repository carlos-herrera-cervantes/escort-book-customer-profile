using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EscortBookCustomerProfile.Constants;
using Newtonsoft.Json;

namespace EscortBookCustomerProfile.Models
{
    public class Profile : Base
    {
        #region snippet_Properties

        public string ID { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [JsonProperty("customerId")]
        public string CustomerID { get; set; }

        [Required]
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [Required]
        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [JsonProperty("email")]
        public string Email { get; set; }

        [Required]
        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; } = Genders.NotSpecified;

        [JsonProperty("birthdate")]
        public DateTime Birthdate { get; set; }

        #endregion

        #region snippet_ForeignProperties

        public ICollection<Photo> Photos { get; set; }

        public Avatar Avatar { get; set; }

        public ICollection<Identification> Identifications { get; set; }

        public ProfileStatus ProfileStatus { get; set; }

        #endregion
    }
}