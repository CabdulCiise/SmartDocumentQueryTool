using API.Core.Models;
using API.Core.Services;
using API.Data.Contexts;
using AutoMapper;

namespace API.Infrastructure.Services;

public class RoleService(ApiContext apiContext, IMapper mapper) : IRoleService
{
    public List<Role> GetRoles()
    {
        return mapper.Map<List<Role>>(apiContext.Roles.ToList());
    }
}
