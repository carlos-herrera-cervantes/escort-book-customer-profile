using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EscortBookCustomerProfile.Models
{
    [Table("profile_status_category", Schema = "public")]
    public class ProfileStatusCategory : Base
    {
        #region snippet_Properties

        [Column("id")]
        public string ID { get; set; } = Guid.NewGuid().ToString();

        [Column("name")]
        public string Name { get; set; }

        [Column("active")]
        public bool Active { get; set; }

        #endregion
    }
}