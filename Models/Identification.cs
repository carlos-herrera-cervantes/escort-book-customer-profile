using System;

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
}