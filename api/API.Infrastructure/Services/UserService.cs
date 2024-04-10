using API.Core.DTOs;
using API.Core.Models;
using API.Core.Services;
using API.Data.Contexts;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Infrastructure.Services;

public class UserService(ApiContext apiContext, IMapper mapper) : IUserService
{
    public List<User> GetUsers()
    {
        return mapper.Map<List<User>>(apiContext.Users
            .Include(x => x.Role)
            .Include(x => x.Documents)
            .Include(x => x.Feedbacks)
            .Include(x => x.Chats)
            .OrderBy(x => x.CreatedAt)
            .ToList());
    }

    public User GetUser(int userId)
    {
        return mapper.Map<User>(apiContext.Users
            .Where(x => x.Id == userId)
            .Include(x => x.Role)
            .Include(x => x.Documents)
            .Include(x => x.Feedbacks)
            .Include(x => x.Chats)
            .OrderBy(x => x.CreatedAt)
            .FirstOrDefault());
    }

    public User UpdateCustomInstruction(int userId, string customInstruction)
    {
        var entity = apiContext.Users.FirstOrDefault(x => x.Id == userId);

        if (entity != null)
        {
            entity.CustomInstruction = customInstruction;
            apiContext.SaveChanges();
        }

        return mapper.Map<User>(entity);
    }   

    public User UpdateUserRole(int userId, int newRoleId)
    {
        var entity = apiContext.Users.FirstOrDefault(x => x.Id == userId);

        if (entity != null)
        {
            entity.RoleId = newRoleId;
            apiContext.SaveChanges();
        }

        return mapper.Map<User>(entity);
    }

    public void DeleteUser(int id)
    {
        var entity = apiContext.Users.FirstOrDefault(x => x.Id == id);

        if (entity != null)
        {
            apiContext.Users.Remove(entity);
            apiContext.SaveChanges();
        }
    }

    public User CreateUser(UserDto userDto)
    {
        var existingUser = apiContext.Users.FirstOrDefault(x => x.Username == userDto.Username);

        if (existingUser != null)
        {
            throw new Exception("Username not available. Try a different one.");
        }

        var entity = new Data.Entities.User
        {
            CreatedAt = DateTime.Now,
            Username = userDto.Username,
            Password = userDto.Password,
            CustomInstruction = "",
            RoleId = 2,
        };

        apiContext.Users.Add(entity);
        apiContext.SaveChanges();

        return Login(userDto);
    }

    public User Login(UserDto userDto)
    {
        var entity = apiContext.Users
            .Include(x => x.Role)
            .FirstOrDefault(x => x.Username == userDto.Username && x.Password == userDto.Password);

        return mapper.Map<User>(entity);
    }
}
