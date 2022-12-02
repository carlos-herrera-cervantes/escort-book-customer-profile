using Newtonsoft.Json;
using System.Collections.Generic;

namespace EscortBookCustomerProfile.Web.Models;

public class Pagination
{
    #region snippet_Properties

    [JsonProperty("page")]
    public int Page { get; set; } = 1;

    [JsonProperty("pageSize")]
    public int PageSize { get; set; } = 10;

    [JsonProperty("email")]
    public string Email { get; set; }

    #endregion

    #region snippet_Deconstructors

    public void Deconstruct(out int page, out int pageSize, out string email)
        => (page, pageSize, email) = (Page, PageSize, Email);

    #endregion
}

public class PaginationResult<T> where T : class
{
    [JsonProperty("previous")]
    public int Previous { get; set; }

    [JsonProperty("next")]
    public int Next { get; set; }

    [JsonProperty("total")]
    public int Total { get; set; }

    [JsonProperty("data")]
    public IEnumerable<T> Data { get; set; }

    public PaginationResult<T> CalculatePagination(int page, int pageSize)
    {
        var current = page == 0 ? 1 * pageSize : page * pageSize;
        Next = current < Total ? page + 1 : 0;
        Previous = current > pageSize ? page - 1 : 0;
        return this;
    }
}
