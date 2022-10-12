using System.Collections.Generic;
using System.Threading.Tasks;
using EscortBookCustomerProfile.Models;

namespace EscortBookCustomerProfile.Repositories;

public interface IProfileRepository
{
    Task<IEnumerable<Profile>> GetAllAsync(int page, int pageSize);

    Task<Profile> GetByIdAsync(string id);

    Task CreateAsync(Profile profile);

    Task UpdateByIdAsync(Profile profile);

    Task<int> CountAsync();
}
