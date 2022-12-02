using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Linq;
using System;
using EscortBookCustomerProfile.Web.Contexts;
using EscortBookCustomerProfile.Web.Models;

namespace EscortBookCustomerProfile.Web.Repositories;

public class ProfileStatusCategoryRepository : IProfileStatusCategoryRepository
{
    #region snippet_Properties

    private readonly EscortBookCustomerProfileContext _context;

    #endregion

    #region snippet_Constructors

    public ProfileStatusCategoryRepository(EscortBookCustomerProfileContext context)
        => _context = context;

    #endregion

    #region snippet_ActionMethods

    public async Task<IEnumerable<ProfileStatusCategory>> GetAllAsync()
        => await _context.ProfileStatusCategories.ToListAsync();

    public async Task<ProfileStatusCategory> GetAsync(Expression<Func<ProfileStatusCategory, bool>> expression)
        => await _context.ProfileStatusCategories
            .AsNoTracking()
            .FirstOrDefaultAsync(expression);

    public async Task CreateAsync(ProfileStatusCategory category)
    {
        await _context.ProfileStatusCategories.AddAsync(category);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Expression<Func<ProfileStatusCategory, bool>> expression)
    {
        var candidatesToDelete = await _context.ProfileStatusCategories.Where(expression).ToListAsync();
        _context.ProfileStatusCategories.RemoveRange(candidatesToDelete);
        await _context.SaveChangesAsync();
    }

    #endregion
}
