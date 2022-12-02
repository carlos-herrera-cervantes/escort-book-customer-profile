using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EscortBookCustomerProfile.Web.Models;

namespace EscortBookCustomerProfile.Web.Repositories;

public interface IIdentificationPartRepository
{
    Task<IEnumerable<IdentificationPart>> GetAllAsync();

    Task<IdentificationPart> GetAsync(Expression<Func<IdentificationPart, bool>> expression);
}
