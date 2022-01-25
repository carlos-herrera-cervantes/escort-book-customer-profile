using System;
using System.Collections.Generic;

namespace EscortBookCustomerProfile.Models
{
    public class IdentificationPart : Base
    {
        #region snippet_Properties

        public string IdentificationPartID { get; set; } = Guid.NewGuid().ToString();

        public string Name { get; set; }

        #endregion

        #region snippet_ForeignProperties

        public ICollection<Identification> Identifications { get; set; }

        #endregion
    }
}