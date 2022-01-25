using System;
using System.Collections.Generic;

namespace EscortBookCustomerProfile.Models
{
    public class ProfileStatusCategory : Base
    {
        #region snippet_Properties

        public string ID { get; set; } = Guid.NewGuid().ToString();

        public string Name { get; set; }

        public bool Active { get; set; }

        #endregion

        #region snippet_ForeignProperties

        public ICollection<ProfileStatus> ProfileStatus { get; set; }

        #endregion
    }
}