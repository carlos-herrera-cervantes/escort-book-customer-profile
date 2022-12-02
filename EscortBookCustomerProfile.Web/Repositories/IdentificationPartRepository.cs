using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System;
using EscortBookCustomerProfile.Web.Contexts;
using EscortBookCustomerProfile.Web.Models;

namespace EscortBookCustomerProfile.Web.Repositories;

public class IdentificationPartRepository : IIdentificationPartRepository
{
    #region snippet_Properties

    private readonly EscortBookCustomerProfileContext _context;

    #endregion

    #region snippet_Constructors

    public IdentificationPartRepository(EscortBookCustomerProfileContext context)
        => _context = context;

    #endregion

    #region snippet_ActionMethods

    public async Task<IEnumerable<IdentificationPart>> GetAllAsync()
        => await this._context.IdentificationParts.ToListAsync();

    public async Task<IdentificationPart> GetAsync(Expression<Func<IdentificationPart, bool>> expression)
        => await this._context.IdentificationParts.AsNoTracking().FirstOrDefaultAsync(expression);

    #endregion
}
