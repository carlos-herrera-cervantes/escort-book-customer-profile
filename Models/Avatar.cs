using System;

namespace EscortBookCustomerProfile.Models
{
    public class Avatar : Base
    {
        #region snippet_Properties

        public string ID { get; set; } = Guid.NewGuid().ToString();

        public string Path { get; set; }

        public string ProfileID { get; set; }

        #endregion
    }
}