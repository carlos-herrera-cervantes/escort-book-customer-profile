using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System;
using EscortBookCustomerProfile.Web.Contexts;
using EscortBookCustomerProfile.Web.Models;

namespace EscortBookCustomerProfile.Web.Repositories;

public class ProfileRepository : IProfileRepository
{
    #region snippet_Properties

    private readonly EscortBookCustomerProfileContext _context;

    #endregion

    #region snippet_Constructors

    public ProfileRepository(EscortBookCustomerProfileContext context)
        => _context = context;

    #endregion

    #region snippet_ActionMethods

    public async Task<IEnumerable<Profile>> GetAllAsync(int page, int pageSize)
        => await _context.Profiles.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

    public async Task<Profile> GetAsync(Expression<Func<Profile, bool>> expression)
        => await _context.Profiles.AsNoTracking().FirstOrDefaultAsync(expression);

    public async Task CreateAsync(Profile profile)
    {
        await _context.Profiles.AddAsync(profile);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Profile profile)
    {
        _context.Entry(profile).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task<int> CountAsync(Expression<Func<Profile, bool>> expression)
        => await _context.Profiles.CountAsync(expression);

    public async Task DeleteAsync(Expression<Func<Profile, bool>> expression)
    {
        var candidatesToDelete = await _context.Profiles.Where(expression).ToListAsync();
        _context.Profiles.RemoveRange(candidatesToDelete);
        await _context.SaveChangesAsync();
    }

    #endregion
}
