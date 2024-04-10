using API.Core.DTOs;
using API.Core.Models;
using API.Core.Services;
using API.Web.Helpers;
using API.Web.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Web.Controllers;

public class UserController(IUserService userService, IConfiguration configuration) : BaseApiController
{
    [Authorize(Policy = IdentityData.AdminUserPolicyName)]
    [HttpGet]
    public List<User> GetUsers()
    {
        return userService.GetUsers();
    }

    [HttpGet("{userId}")]
    public User GetUser(int userId)
    {
        return userService.GetUser(userId);
    }

    [HttpPut("{userId}/CustomInstruction/{customInstruction}")]
    public User UpdateCustomInstruction(int userId, string customInstruction)
    {
        return userService.UpdateCustomInstruction(userId, customInstruction);
    }

    [Authorize(Policy = IdentityData.AdminUserPolicyName)]
    [HttpDelete("{userId}")]
    public void DeleteUser(int userId)
    {
        userService.DeleteUser(userId);
    }

    [Authorize(Policy = IdentityData.AdminUserPolicyName)]
    [HttpPut("{userId}/Role/{newRoleId}")]
    public User UpdateUser(int userId, int newRoleId)
    {
        return userService.UpdateUserRole(userId, newRoleId);
    }

    [AllowAnonymous]
    [HttpPost("Register")]
    public IActionResult RegisterUser(UserDto userDto)
    {
        try
        {
            var user = userService.CreateUser(userDto);

            if (user != null)
            {
                var token = TokenGenerator.GenerateToken(user, configuration);
                return Ok(token);
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return Unauthorized();
    }

    [AllowAnonymous]
    [HttpPost("Login")]
    public IActionResult Login(UserDto userDto)
    {
        var user = userService.Login(userDto);

        if (user != null)
        {
            var token = TokenGenerator.GenerateToken(user, configuration);
            return Ok(token);
        }

        return Unauthorized();
    }

    [HttpGet("Validate")]
    public IActionResult Validate()
    {
        return Ok();
    }
}
