using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EscortBookCustomerProfile.Models
{
    [Table("identification_part", Schema = "public")]
    public class IdentificationPart : Base
    {
        #region snippet_Properties

        [Column("id")]
        public string ID { get; set; } = Guid.NewGuid().ToString();

        [Column("name")]
        public string Name { get; set; }

        #endregion
    }
}