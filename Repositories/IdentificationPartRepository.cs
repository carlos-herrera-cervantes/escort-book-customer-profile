using System.Collections.Generic;
using System.Threading.Tasks;
using EscortBookCustomerProfile.Contexts;
using EscortBookCustomerProfile.Models;
using Microsoft.EntityFrameworkCore;

namespace EscortBookCustomerProfile.Repositories
{
    public class IdentificationPartRepository : IIdentificationPartRepository
    {
        #region snippet_Properties

        private readonly EscortBookCustomerProfileContext _context;

        #endregion

        #region snippet_Constructors

        public IdentificationPartRepository(EscortBookCustomerProfileContext context)
            => _context = context;

        #endregion

        #region snippet_ActionMethods

        public async Task<IEnumerable<IdentificationPart>> GetAllAsync()
            => await this._context.IdentificationParts.ToListAsync();

        #endregion
    }
}