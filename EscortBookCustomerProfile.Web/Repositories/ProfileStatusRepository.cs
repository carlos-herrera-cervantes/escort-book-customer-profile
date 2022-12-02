using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System;
using System.Linq;
using EscortBookCustomerProfile.Web.Contexts;
using EscortBookCustomerProfile.Web.Models;

namespace EscortBookCustomerProfile.Web.Repositories;

public class ProfileStatusRepository : IProfileStatusRepository
{
    #region snippet_Properties

    private readonly EscortBookCustomerProfileContext _context;

    #endregion

    #region snippet_Constructors

    public ProfileStatusRepository(EscortBookCustomerProfileContext context)
        => _context = context;

    #endregion

    #region snippet_ActionMethods

    public async Task<ProfileStatus> GetAsync(Expression<Func<ProfileStatus, bool>> expression)
        => await _context.ProfileStatus.AsNoTracking().FirstOrDefaultAsync(expression);

    public async Task<int> CountAsync(Expression<Func<ProfileStatus, bool>> expression)
        => await _context.ProfileStatus.CountAsync(expression);

    public async Task CreateAsync(ProfileStatus profileStatus)
    {
        await _context.ProfileStatus.AddAsync(profileStatus);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ProfileStatus profileStatus)
    {
        _context.Entry(profileStatus).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Expression<Func<ProfileStatus, bool>> expression)
    {
        var candidatesToDelete = await _context.ProfileStatus.Where(expression).ToListAsync();
        _context.ProfileStatus.RemoveRange(candidatesToDelete);
        await _context.SaveChangesAsync();
    }

    #endregion
}
