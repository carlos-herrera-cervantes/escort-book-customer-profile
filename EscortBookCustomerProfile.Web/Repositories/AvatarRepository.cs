using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System;
using System.Linq;
using EscortBookCustomerProfile.Web.Contexts;
using EscortBookCustomerProfile.Web.Models;

namespace EscortBookCustomerProfile.Web.Repositories;

public class AvatarRepository : IAvatarRepository
{
    #region snippet_Properties

    private readonly EscortBookCustomerProfileContext _context;

    #endregion

    #region snippet_Constructors

    public AvatarRepository(EscortBookCustomerProfileContext context)
        => _context = context;

    #endregion

    #region snippet_ActionMethods

    public async Task<Avatar> GetAsync(Expression<Func<Avatar, bool>> expression)
        => await _context.Avatars.AsNoTracking().FirstOrDefaultAsync(expression);

    public async Task<int> CountAsync(Expression<Func<Avatar, bool>> expression)
        => await _context.Avatars.CountAsync(expression);

    public async Task CreateAsync(Avatar avatar)
    {
        await _context.Avatars.AddAsync(avatar);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Avatar avatar, JsonPatchDocument<Avatar> currentAvatar)
    {
        currentAvatar.ApplyTo(avatar);
        _context.Entry(avatar).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Expression<Func<Avatar, bool>> expression)
    {
        var candidatesToDelete = await _context.Avatars.Where(expression).ToListAsync();
        _context.Avatars.RemoveRange(candidatesToDelete);
        await _context.SaveChangesAsync();
    }

    #endregion
}
