using System.Threading.Tasks;
using EscortBookCustomerProfile.Contexts;
using EscortBookCustomerProfile.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace EscortBookCustomerProfile.Repositories
{
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

        public async Task<Profile> GetByIdAsync(string customerId)
            => await _context.Profiles.AsNoTracking().FirstOrDefaultAsync(p => p.ID == customerId);

        public async Task CreateAsync(Profile profile)
        {
            await _context.Profiles.AddAsync(profile);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateByIdAsync(Profile profile, JsonPatchDocument<Profile> currentProfile)
        {
            currentProfile.ApplyTo(profile);
            _context.Entry(profile).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        #endregion
    }
}