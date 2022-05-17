using EscortBookCustomerProfile.Models;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EscortBookCustomerProfile.Repositories
{
    public interface IIdentificationRepository
    {
        Task<IEnumerable<Identification>> GetAllByCustomerAsync(string profileId);

        Task<Identification> GetByIdAsync(string profileId, string partId);

        Task CreateAsync(Identification identification);

        Task UpdateByIdAsync(Identification identification, JsonPatchDocument<Identification> currentIdentification);
    }
}
