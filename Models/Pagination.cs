using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace EscortBookCustomerProfile.Models
{
    public class Pagination
    {
        #region snippet_Properties

        [JsonProperty("page")]
        public int Page { get; set; } = 1;

        [JsonProperty("pageSize")]
        public int PageSize { get; set; } = 10;

        #endregion

        #region snippet_Deconstructors

        public void Deconstruct(out int page, out int pageSize)
            => (page, pageSize) = (Page, PageSize);

        #endregion
    }
}
