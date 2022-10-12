using System.Collections.Generic;
using System.Threading.Tasks;
using EscortBookCustomerProfile.Models;

namespace EscortBookCustomerProfile.Repositories;

public interface IPhotoRepository
{
    Task<IEnumerable<Photo>> GetAllAsync(string profileId, int page, int pageSize);

    Task<Photo> GetByIdAsync(string profileId, string id);

    Task CreateAsync(Photo photo);

    Task DeleteByIdAsync(string id);
}
