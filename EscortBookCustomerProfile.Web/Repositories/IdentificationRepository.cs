using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System;
using EscortBookCustomerProfile.Web.Contexts;
using EscortBookCustomerProfile.Web.Models;

namespace EscortBookCustomerProfile.Web.Repositories;

public class IdentificationRepository : IIdentificationRepository
{
    #region snippet_Properties

    private readonly EscortBookCustomerProfileContext _context;

    #endregion

    #region snippet_Constructors

    public IdentificationRepository(EscortBookCustomerProfileContext context)
        => _context = context;

    #endregion

    #region snippet_ActionMethods

    public async Task<IEnumerable<Identification>> GetAllAsync(Expression<Func<Identification, bool>> expression)
        => await _context.Identifications.Where(expression).ToListAsync();

    public async Task<Identification> GetAsync(Expression<Func<Identification, bool>> expression)
        => await _context.Identifications
            .AsNoTracking()
            .FirstOrDefaultAsync(expression);

    public async Task<int> CountAsync(Expression<Func<Identification, bool>> expression)
        => await _context.Identifications.CountAsync(expression);

    public async Task CreateAsync(Identification identification)
    {
        await _context.Identifications.AddAsync(identification);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Identification identification, JsonPatchDocument<Identification> currentIdentification)
    {
        currentIdentification.ApplyTo(identification);
        _context.Entry(identification).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Expression<Func<Identification, bool>> expression)
    {
        var candidatesToDelete = await _context.Identifications.Where(expression).ToListAsync();
        _context.Identifications.RemoveRange(candidatesToDelete);
        await _context.SaveChangesAsync();
    }

    #endregion
}
