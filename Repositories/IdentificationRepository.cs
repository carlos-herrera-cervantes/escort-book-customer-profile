using EscortBookCustomerProfile.Contexts;
using EscortBookCustomerProfile.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EscortBookCustomerProfile.Repositories
{
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

        public async Task<Identification> GetByIdAsync(string profileId, string partId)
            => await _context.Identifications.AsNoTracking().FirstOrDefaultAsync(i => i.CustomerID == profileId && i.IdentificationPartID == partId);

        public async Task CreateAsync(Identification identification)
        {
            await _context.Identifications.AddAsync(identification);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateByIdAsync(Identification identification, JsonPatchDocument<Identification> currentIdentification)
        {
            currentIdentification.ApplyTo(identification);
            _context.Entry(identification).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        #endregion
    }
}
