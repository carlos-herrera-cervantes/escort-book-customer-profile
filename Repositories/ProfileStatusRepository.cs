using System.Threading.Tasks;
using EscortBookCustomerProfile.Contexts;
using EscortBookCustomerProfile.Models;
using Microsoft.EntityFrameworkCore;

namespace EscortBookCustomerProfile.Repositories;

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

    public async Task<ProfileStatus> GetByIdAsync(string profileId)
        => await _context.ProfileStatus.AsNoTracking().FirstOrDefaultAsync(p => p.CustomerID == profileId);

    public async Task CreateAsync(ProfileStatus profileStatus)
    {
        await _context.ProfileStatus.AddAsync(profileStatus);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateByIdAsync(ProfileStatus profileStatus)
    {
        _context.Entry(profileStatus).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    #endregion
}
