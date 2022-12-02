using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System;
using EscortBookCustomerProfile.Web.Models;

namespace EscortBookCustomerProfile.Web.Repositories;

public interface IIdentificationRepository
{
    Task<IEnumerable<Identification>> GetAllAsync(Expression<Func<Identification, bool>> expression);

    Task<Identification> GetAsync(Expression<Func<Identification, bool>> expression);

    Task<int> CountAsync(Expression<Func<Identification, bool>> expression);

    Task CreateAsync(Identification identification);

    Task UpdateAsync(Identification identification, JsonPatchDocument<Identification> currentIdentification);

    Task DeleteAsync(Expression<Func<Identification, bool>> expression);
}
