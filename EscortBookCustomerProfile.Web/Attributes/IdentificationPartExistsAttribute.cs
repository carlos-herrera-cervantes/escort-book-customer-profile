using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using EscortBookCustomerProfile.Web.Repositories;

namespace EscortBookCustomerProfile.Web.Attributes;

public class IdentificationPartExistsAttribute : TypeFilterAttribute
{
    public IdentificationPartExistsAttribute() : base(typeof(IdentificationPartExistsFilter)) { }
}

internal class IdentificationPartExistsFilter : IAsyncActionFilter
{
    private readonly IIdentificationPartRepository _identificationPartRepository;

    public IdentificationPartExistsFilter(IIdentificationPartRepository identificationPartRepository)
        => _identificationPartRepository = identificationPartRepository;

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (context.HttpContext.Request.HasFormContentType)
        {
            context.Result = new BadRequestObjectResult(new
            {
                Message = "Content type is not form"
            });
            return;
        }

        var categoryId = context
            .HttpContext
            .Request
            .Form
            .Keys
            .FirstOrDefault(k => k == "identificationPartId") ??
            context.ActionArguments["identificationPartID"] as string;

        if (categoryId is null)
        {
            context.Result = new BadRequestObjectResult(new
            {
                Message = "Missing identification part ID"
            });
            return;
        }

        var category = await _identificationPartRepository.GetAsync(c => c.ID == categoryId);

        if (category is null)
        {
            context.Result = new NotFoundObjectResult(new
            {
                Message = "Category not found"
            });
            return;
        }

        await next();
    }
}
