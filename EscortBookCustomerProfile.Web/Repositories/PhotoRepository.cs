using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System;
using EscortBookCustomerProfile.Web.Contexts;
using EscortBookCustomerProfile.Web.Models;

namespace EscortBookCustomerProfile.Web.Repositories;

public class PhotoRepository : IPhotoRepository
{
    #region snippet_Properties

    private readonly EscortBookCustomerProfileContext _context;

    #endregion

    #region snippet_Constructors

    public PhotoRepository(EscortBookCustomerProfileContext context)
        => _context = context;

    #endregion

    #region snippet_ActionMethods

    public async Task<IEnumerable<Photo>> GetAllAsync(Expression<Func<Photo, bool>> expression, int page, int pageSize)
        => await _context.Photos.Where(expression).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

    public async Task<Photo> GetAsync(Expression<Func<Photo, bool>> expression)
        => await _context.Photos.AsNoTracking().FirstOrDefaultAsync(expression);

    public async Task<int> CountAsync(Expression<Func<Photo, bool>> expression)
        => await _context.Photos.CountAsync(expression);

    public async Task CreateAsync(Photo photo)
    {
        await _context.AddAsync(photo);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Expression<Func<Photo, bool>> expression)
    {
        var cadidatesToDelete = await _context.Photos.Where(expression).ToListAsync();
        _context.Photos.RemoveRange(cadidatesToDelete);
        await _context.SaveChangesAsync();
    }

    #endregion
}
