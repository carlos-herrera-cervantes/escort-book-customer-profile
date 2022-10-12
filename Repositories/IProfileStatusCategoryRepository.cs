using System.Collections.Generic;
using EscortBookCustomerProfile.Models;
using System.Threading.Tasks;

namespace EscortBookCustomerProfile.Repositories;

public interface IProfileStatusCategoryRepository
{
    Task<IEnumerable<ProfileStatusCategory>> GetAllAsync();

    Task<ProfileStatusCategory> GetByName(string name);

    Task<ProfileStatusCategory> GetByIdAsync(string id);
}
