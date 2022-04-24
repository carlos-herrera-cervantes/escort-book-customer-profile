using System;
using System.ComponentModel.DataAnnotations.Schema;
using EscortBookCustomerProfile.Constants;
using Newtonsoft.Json;

namespace EscortBookCustomerProfile.Models
{
    [Table("profile", Schema = "public")]
    public class Profile : Base
    {
        #region snippet_Properties

        [Column("id")]
        public string ID { get; set; } = Guid.NewGuid().ToString();

        [Column("customer_id")]
        [JsonProperty("customerId")]
        public string CustomerID { get; set; }

        [Column("first_name")]
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [Column("last_name")]
        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [Column("email")]
        [JsonProperty("email")]
        public string Email { get; set; }

        [Column("phone_number")]
        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [Column("gender")]
        [JsonProperty("gender")]
        public string Gender { get; set; } = Genders.NotSpecified;

        [Column("birthdate")]
        [JsonProperty("birthdate")]
        public DateTime Birthdate { get; set; }

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
    }
}