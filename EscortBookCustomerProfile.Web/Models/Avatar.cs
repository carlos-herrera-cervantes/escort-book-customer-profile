using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EscortBookCustomerProfile.Web.Models;

[Table("avatar", Schema = "public")]
public class Avatar : Base
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
