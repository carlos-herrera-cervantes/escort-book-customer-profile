using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EscortBookCustomerProfile.Web.Models;

namespace EscortBookCustomerProfile.Web.Repositories;

public interface IProfileStatusCategoryRepository
{
    Task<IEnumerable<ProfileStatusCategory>> GetAllAsync();

    Task<ProfileStatusCategory> GetAsync(Expression<Func<ProfileStatusCategory, bool>> expression);

    Task CreateAsync(ProfileStatusCategory category);

    Task DeleteAsync(Expression<Func<ProfileStatusCategory, bool>> expression);
}
