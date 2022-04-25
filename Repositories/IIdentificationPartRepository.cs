using System.Collections.Generic;
using System.Threading.Tasks;
using EscortBookCustomerProfile.Models;

namespace EscortBookCustomerProfile.Repositories
{
    public interface IIdentificationPartRepository
    {
         Task<IEnumerable<IdentificationPart>> GetAllAsync();

        Task<IdentificationPart> GetByIdAsync(string id);
    }
}