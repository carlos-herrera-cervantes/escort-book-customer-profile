using System;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace EscortBookCustomerProfile.Models
{
    [Table("photo", Schema = "public")]
    public class Photo : Base
    {
        #region snippet_Properties

        [Column("id")]
        public string ID { get; set; } = Guid.NewGuid().ToString();

        [Column("path")]
        public string Path { get; set; }

        [Column("customer_id")]
        public string CustomerID { get; set; }

        #endregion
    }
}