using EscortBookCustomerProfile.Contexts;
using EscortBookCustomerProfile.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EscortBookCustomerProfile.Repositories
{
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

        public async Task<ProfileStatusCategory> GetByName(string name)
            => await _context.ProfileStatusCategories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Name == name);

        public async Task<ProfileStatusCategory> GetByIdAsync(string id)
            => await _context.ProfileStatusCategories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.ID == id);

        #endregion
    }
}
