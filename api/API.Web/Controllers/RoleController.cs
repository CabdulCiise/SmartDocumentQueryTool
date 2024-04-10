using API.Core.Models;
using API.Core.Services;
using API.Web.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Web.Controllers;

[Authorize(Policy = IdentityData.AdminUserPolicyName)]
public class RoleController(IRoleService roleService) : BaseApiController
{
    [HttpGet]
    public List<Role> GetRoles()
    {
        return roleService.GetRoles();
    }   
}
