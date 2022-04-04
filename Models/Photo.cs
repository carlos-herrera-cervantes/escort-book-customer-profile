using System;
using Newtonsoft.Json;

namespace EscortBookCustomerProfile.Models
{
    public class Photo : Base
    {
        #region snippet_Properties

        public string ID { get; set; } = Guid.NewGuid().ToString();

        public string Path { get; set; }

        public string CustomerID { get; set; }

        #endregion

        #region snippet_ForeignProperties

        [JsonIgnore]
        public Profile Customer { get; set; }

        #endregion
    }
}