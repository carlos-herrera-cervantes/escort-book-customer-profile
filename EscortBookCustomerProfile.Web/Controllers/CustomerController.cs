using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EscortBookCustomerProfile.Web.Models;
using EscortBookCustomerProfile.Web.Repositories;

namespace EscortBookCustomerProfile.Web.Controllers;

[Route("api/v1/customers")]
[ApiController]
public class CustomerController : ControllerBase
{
    #region snippet_Properties

    private readonly IProfileRepository _profileRepository;

    #endregion

    #region snippet_Constructors

    public CustomerController(IProfileRepository profileRepository)
        => _profileRepository = profileRepository;

    #endregion

    #region snippet_ActionMethods

    [HttpGet]
    public async Task<IActionResult> GetAllAsync([FromQuery] Pagination pagination)
    {
        var (page, pageSize, _) = pagination;
        var profiles = await _profileRepository.GetAllAsync(page, pageSize);
        var totalRows = await _profileRepository.CountAsync(p => p.ID != string.Empty);
        var paginationResult = new PaginationResult<Profile>
        {
            Total = totalRows,
            Data = profiles
        };
        return Ok(paginationResult.CalculatePagination(page, pageSize));
    }

    #endregion
}
