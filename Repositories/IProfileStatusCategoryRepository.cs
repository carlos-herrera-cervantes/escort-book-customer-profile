using EscortBookCustomerProfile.Models;
using System.Threading.Tasks;

namespace EscortBookCustomerProfile.Repositories
{
    public interface IProfileStatusCategoryRepository
    {
        Task<ProfileStatusCategory> GetByName(string name);

        Task<ProfileStatusCategory> GetByIdAsync(string id);
    }
}
