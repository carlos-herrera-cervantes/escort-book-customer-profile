using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EscortBookCustomerProfile.Contexts;
using EscortBookCustomerProfile.Models;
using Microsoft.EntityFrameworkCore;

namespace EscortBookCustomerProfile.Repositories
{
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

        public async Task<IEnumerable<Photo>> GetAllAsync(string profileId, int page, int pageSize)
            => await _context.Photos.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        public async Task<Photo> GetByIdAsync(string profileId, string id)
            => await _context.Photos.AsNoTracking().FirstOrDefaultAsync(p => p.ProfileID == profileId && p.ID == id);

        public async Task CreateAsync(Photo photo)
        {
            await _context.AddAsync(photo);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(string id)
        {
            _context.Photos.Remove(new Photo { ID = id });
            await _context.SaveChangesAsync();
        }

        #endregion
    }
}