using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace EscortBookCustomerProfile.Models
{
    public class IdentificationPart : Base
    {
        #region snippet_Properties

        public string ID { get; set; } = Guid.NewGuid().ToString();

        public string Name { get; set; }

        #endregion

        #region snippet_ForeignProperties

        [JsonIgnore]
        public ICollection<Identification> Identifications { get; set; }

        #endregion
    }
}