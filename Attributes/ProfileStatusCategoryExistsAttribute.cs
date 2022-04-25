using System.IO;
using System.Threading.Tasks;
using EscortBookCustomerProfile.Models;
using EscortBookCustomerProfile.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using EscortBookCustomerProfile.Constants;
using System.Collections.Generic;
using System.Linq;

namespace EscortBookCustomerProfile.Attributes
{
    public class ProfileStatusCategoryExistsAttribute : TypeFilterAttribute
    {
        public ProfileStatusCategoryExistsAttribute() : base(typeof(ProfileStatusCategoryExistsFilter)) {}

        private class ProfileStatusCategoryExistsFilter : IAsyncActionFilter
        {
            private readonly IProfileStatusCategoryRepository _profileStatusCategoryRepository;

            public ProfileStatusCategoryExistsFilter(IProfileStatusCategoryRepository profileStatusCategoryRepository)
                => _profileStatusCategoryRepository = profileStatusCategoryRepository;

            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                using var reader = new StreamReader(context.HttpContext.Request.Body);
                var content = await reader.ReadToEndAsync();
                var profileStatus = JsonConvert.DeserializeObject<UpdateProfileStatusDTO>(content);

                var category = await _profileStatusCategoryRepository.GetByIdAsync(profileStatus.ProfileStatusCategoryID);

                if (category is null)
                {
                    context.Result = new NotFoundResult();
                    return;
                }

                var validStatus = new List<string> { ValidProfileStatus.Deactivated, ValidProfileStatus.Deleted };
                var isValid = validStatus.FirstOrDefault(s => s == category.Name);

                if (isValid is null)
                {
                    context.Result = new BadRequestResult();
                    return;
                }

                await next();
            }
        }
    }
}
