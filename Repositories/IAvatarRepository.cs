using System.Threading.Tasks;
using EscortBookCustomerProfile.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace EscortBookCustomerProfile.Repositories
{
    public interface IAvatarRepository
    {
        Task<Avatar> GetByIdAsync(string profileId);

        Task CreateAsync(Avatar avatar);

        Task UpdateByIdAsync(Avatar avatar, JsonPatchDocument<Avatar> currentAvatar);

        Task DeleteByIdAsync(string id);
    }
}