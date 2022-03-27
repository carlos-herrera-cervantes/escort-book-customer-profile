using System;
using System.Collections.Generic;
using EscortBookCustomerProfile.Constants;
using EscortBookCustomerProfile.Types;
using Newtonsoft.Json;

namespace EscortBookCustomerProfile.Models
{
    public class Profile : Base
    {
        #region snippet_Properties

        public string ID { get; set; } = Guid.NewGuid().ToString();

        [JsonProperty("customerId")]
        public string CustomerID { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; } = Genders.NotSpecified;

        [JsonProperty("birthdate")]
        public DateTime Birthdate { get; set; }

        #endregion

        #region snippet_ForeignProperties

        [JsonIgnore]
        public ICollection<Photo> Photos { get; set; }

        [JsonIgnore]
        public Avatar Avatar { get; set; }

        [JsonIgnore]
        public ICollection<Identification> Identifications { get; set; }

        [JsonIgnore]
        public ProfileStatus ProfileStatus { get; set; }

        #endregion
    }

    public class UpdateProfileDTO
    {
        #region snippet_Properties

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; } = Genders.NotSpecified;

        [JsonProperty("birthdate")]
        public DateTime? Birthdate { get; set; }

        #endregion

        #region snippet_JwtProperties

        [JsonProperty("user")]
        public DecodedJwt User { get; set; }

        #endregion
    }
}