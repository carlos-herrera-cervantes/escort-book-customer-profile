using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using System.Linq.Expressions;
using System;
using EscortBookCustomerProfile.Web.Models;

namespace EscortBookCustomerProfile.Web.Repositories;

public interface IAvatarRepository
{
    Task<Avatar> GetAsync(Expression<Func<Avatar, bool>> expression);

    Task<int> CountAsync(Expression<Func<Avatar, bool>> expression);

    Task CreateAsync(Avatar avatar);

    Task UpdateAsync(Avatar avatar, JsonPatchDocument<Avatar> currentAvatar);

    Task DeleteAsync(Expression<Func<Avatar, bool>> expression);
}
