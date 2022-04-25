using System.Linq;
using System.Threading.Tasks;
using EscortBookCustomerProfile.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EscortBookCustomerProfile.Attributes
{
    public class IdentificationPartExistsAttribute : TypeFilterAttribute
    {
        public IdentificationPartExistsAttribute() : base(typeof(IdentificationPartExistsFilter)) {}

        private class IdentificationPartExistsFilter : IAsyncActionFilter
        {
            private readonly IIdentificationPartRepository _identificationPartRepository;

            public IdentificationPartExistsFilter(IIdentificationPartRepository identificationPartRepository)
                => _identificationPartRepository = identificationPartRepository;

            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                if (context.HttpContext.Request.HasFormContentType)
                {
                    context.Result = new BadRequestResult();
                    return;
                }

                var categoryId = context.HttpContext.Request.Form.Keys.FirstOrDefault(k => k == "identificationPartId") ??
                    context.ActionArguments["identificationPartID"] as string;

                if (categoryId is null)
                {
                    context.Result = new BadRequestResult();
                    return;
                }

                var category = await _identificationPartRepository.GetByIdAsync(categoryId);

                if (category is null)
                {
                    context.Result = new NotFoundResult();
                    return;
                }

                await next();
            }
        }
    }
}
