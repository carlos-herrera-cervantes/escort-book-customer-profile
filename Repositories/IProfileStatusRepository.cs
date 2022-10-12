using System.Threading.Tasks;
using EscortBookCustomerProfile.Models;

namespace EscortBookCustomerProfile.Repositories;

public interface IProfileStatusRepository
{
    Task<ProfileStatus> GetByIdAsync(string profileId);

    Task CreateAsync(ProfileStatus profileStatus);

    Task UpdateByIdAsync(ProfileStatus profileStatus);
}
