using System.Threading.Tasks;
using EscortBookCustomerProfile.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EscortBookCustomerProfile.Controllers;

[Route("api/v1/customer/identification-parts")]
[ApiController]
public class IdentificationPartController : ControllerBase
{
    #region snippet_Propertiees

    private readonly IIdentificationPartRepository _identificationPartReporitory;

    #endregion

    #region snippet_Constructors

    public IdentificationPartController(IIdentificationPartRepository identificationPartRepository)
        => _identificationPartReporitory = identificationPartRepository;

    #endregion

    #region snippet_ActionMethods

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
        => Ok(await _identificationPartReporitory.GetAllAsync());

    #endregion
}
