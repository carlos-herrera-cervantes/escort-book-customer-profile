using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EscortBookCustomerProfile.Web.Models;

namespace EscortBookCustomerProfile.Web.Repositories;

public interface IProfileRepository
{
    Task<IEnumerable<Profile>> GetAllAsync(int page, int pageSize);

    Task<Profile> GetAsync(Expression<Func<Profile, bool>> expression);

    Task CreateAsync(Profile profile);

    Task UpdateAsync(Profile profile);

    Task<int> CountAsync(Expression<Func<Profile, bool>> expression);

    Task DeleteAsync(Expression<Func<Profile, bool>> expression);
}
