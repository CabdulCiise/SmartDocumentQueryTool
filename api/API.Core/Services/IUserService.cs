using API.Core.DTOs;
using API.Core.Models;

namespace API.Core.Services;

public interface IUserService
{
    List<User> GetUsers();
    User GetUser(int userId);
    User UpdateCustomInstruction(int userId, string customInstruction);
    User UpdateUserRole(int userId, int newRoleId);
    void DeleteUser(int id);
    User CreateUser(UserDto userDto);
    User Login(UserDto userDto);
}
