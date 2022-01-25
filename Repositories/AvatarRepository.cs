using System.Threading.Tasks;
using EscortBookCustomerProfile.Contexts;
using EscortBookCustomerProfile.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace EscortBookCustomerProfile.Repositories
{
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

        public async Task<Avatar> GetByIdAsync(string profileId)
            => await _context.Avatars.AsNoTracking().FirstOrDefaultAsync(a => a.ProfileID == profileId);

        public async Task CreateAsync(Avatar avatar)
        {
            await _context.Avatars.AddAsync(avatar);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateByIdAsync(Avatar avatar, JsonPatchDocument<Avatar> currentAvatar)
        {
            currentAvatar.ApplyTo(avatar);
            _context.Entry(avatar).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(string profileId)
        {
            _context.Avatars.Remove(new Avatar { ProfileID = profileId });
            await _context.SaveChangesAsync();
        }

        #endregion
    }
}