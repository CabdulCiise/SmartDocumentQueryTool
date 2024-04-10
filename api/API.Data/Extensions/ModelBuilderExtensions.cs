using API.Core.Constants;
using API.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Extensions;

public static class ModelBuilderExtensions
{
    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>().HasData(
            new Role { Id = 1, Name = Roles.Admin },
            new Role { Id = 2, Name = Roles.User }
        );

        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Username = "admin", Password = "admin", CustomInstruction = "", RoleId = 1 },
            new User { Id = 2, Username = "user", Password = "user", CustomInstruction = "", RoleId = 2 }
        );
    }
}