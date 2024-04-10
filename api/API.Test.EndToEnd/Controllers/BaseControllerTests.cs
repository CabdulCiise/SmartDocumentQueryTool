using API.Core.Constants;
using API.Core.Models;
using API.Web.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace API.Test.EndToEnd.Controllers;

public abstract class BaseControllerTests : IDisposable
{
    protected readonly HttpClient client;
    protected readonly WebApplicationFactory<Program> factory;
    protected readonly IConfiguration configuration;
    protected string jwtAdminToken;
    protected string jwtUserToken;
    protected readonly string invalidJwtToken;

    public BaseControllerTests()
    {
        configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.Test.json")
            .Build();

        factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddConfiguration(configuration);
            });
        });

        client = factory.CreateClient();

        var adminUser = new User
        {
            Id = 1,
            Username = "admin",
            CustomInstruction = "test instructions",
            Role = new Role { Name = Roles.Admin }
        };

        var regularUser = new User
        {
            Id = 2,
            Username = "user",
            CustomInstruction = "test instructions",
            Role = new Role { Name = Roles.User }
        };

        jwtAdminToken = TokenGenerator.GenerateToken(adminUser, configuration);
        jwtUserToken = TokenGenerator.GenerateToken(regularUser, configuration);
        invalidJwtToken = string.Empty;
    }

    public void Dispose()
    {
        client.Dispose();
        factory.Dispose();
    }
}
