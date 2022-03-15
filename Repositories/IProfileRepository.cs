using System.Threading.Tasks;
using EscortBookCustomerProfile.Models;

namespace EscortBookCustomerProfile.Repositories
{
    public interface IProfileRepository
    {
        Task<Profile> GetByIdAsync(string id);

        Task CreateAsync(Profile profile);

        Task UpdateByIdAsync(Profile profile);
    }
}