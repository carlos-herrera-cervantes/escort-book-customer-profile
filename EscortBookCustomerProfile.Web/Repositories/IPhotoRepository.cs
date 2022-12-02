using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EscortBookCustomerProfile.Web.Models;

namespace EscortBookCustomerProfile.Web.Repositories;

public interface IPhotoRepository
{
    Task<IEnumerable<Photo>> GetAllAsync(Expression<Func<Photo, bool>> expression, int page, int pageSize);

    Task<Photo> GetAsync(Expression<Func<Photo, bool>> expression);

    Task<int> CountAsync(Expression<Func<Photo, bool>> expression);

    Task CreateAsync(Photo photo);

    Task DeleteAsync(Expression<Func<Photo, bool>> expression);
}
