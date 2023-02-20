using System;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace EscortBookCustomerProfile.Web.Models;

[Table("profile_status", Schema = "public")]
public class ProfileStatus : Base
{
    #region snippet_Properties

    [Column("id")]
    public string ID { get; set; } = Guid.NewGuid().ToString();

    [Column("customer_id")]
    public string CustomerID { get; set; }

    [Column("profile_status_category_id")]
    [JsonProperty("profileStatusCategoryId")]
    public string ProfileStatusCategoryID { get; set; }

    #endregion
}

public class UpdateProfileStatus
{
    #region snippet_Properties

    [JsonProperty("profileStatusCategoryId")]
    public string ProfileStatusCategoryID { get; set; }

    #endregion
}
