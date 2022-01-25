using Newtonsoft.Json;
using System;

namespace EscortBookCustomerProfile.Models
{
    public class ProfileStatus : Base
    {
        #region snippet_Properties

        public string ID { get; set; } = Guid.NewGuid().ToString();

        public string ProfileID { get; set; }

        [JsonProperty("profileStatusCategoryId")]
        public string ProfileStatusCategoryID { get; set; }

        #endregion

        #region snippet_ForeignProperties

        public ProfileStatusCategory ProfileStatusCategory { get; set; }

        #endregion
    }
}