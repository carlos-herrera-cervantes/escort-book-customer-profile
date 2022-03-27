using System;
using Newtonsoft.Json;

namespace EscortBookCustomerProfile.Models
{
    public class Photo : Base
    {
        #region snippet_Properties

        public string ID { get; set; } = Guid.NewGuid().ToString();

        public string Path { get; set; }

        public string ProfileID { get; set; }

        #endregion

        #region snippet_ForeignProperties

        [JsonIgnore]
        public Profile Profile { get; set; }

        #endregion
    }
}