using System.Threading.Tasks;
using EscortBookCustomerProfile.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace EscortBookCustomerProfile.Repositories
{
    public interface IProfileStatusRepository
    {
        Task<ProfileStatus> GetByIdAsync(string profileId);

        Task CreateAsync(ProfileStatus profileStatus);

        Task UpdateByIdAsync(ProfileStatus profileStatus, JsonPatchDocument<ProfileStatus> curentProfileStatus);
    }
}