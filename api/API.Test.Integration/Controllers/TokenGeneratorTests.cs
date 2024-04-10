using API.Core.Constants;
using API.Core.Models;
using API.Web.Helpers;
using API.Web.Identity;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;

namespace API.Test.Integration.Controllers;

[TestFixture]
public class TokenGeneratorTests
{
    private IConfiguration Configuration;

    public TokenGeneratorTests()
    {
        var configValues = new Dictionary<string, string>
            {
                {"Jwt:Key", "MyVeryVeryLongSuperSuperSecretKey"},
                {"Jwt:Issuer", "https://localhost:44322"},
                {"Jwt:Audience", "http://localhost:5173"},
            };

        Configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configValues)
            .Build();
    }

    [Test]
    public void GenerateToken_ValidUser_ReturnsValidToken()
    {
        var user = new User
        {
            Id = 1,
            Username = "test_user",
            CustomInstruction = "custom_instruction",
            Role = new Role
            {
                Id = 1,
                Name = Roles.Admin
            }
        };

        var token = TokenGenerator.GenerateToken(user, Configuration);

        Assert.IsNotNull(token);
        Assert.IsNotEmpty(token);
        Assert.IsTrue(token.Length > 0);
    }

    [Test]
    public void GenerateToken_ValidUser_CorrectClaims()
    {
        var user = new User
        {
            Id = 1,
            Username = "test_user",
            CustomInstruction = "custom_instruction",
            Role = new Role
            {
                Id = 1,
                Name = Roles.Admin
            }
        };

        var token = TokenGenerator.GenerateToken(user, Configuration);
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(token);

        Assert.IsNotNull(jwtToken);

        Assert.That(jwtToken.Payload.Sub, Is.EqualTo(user.Id.ToString()));
        Assert.That(jwtToken.Payload["userId"], Is.EqualTo(user.Id.ToString()));
        Assert.That(jwtToken.Payload["username"], Is.EqualTo(user.Username));
        Assert.That(jwtToken.Payload[IdentityData.AdminUserClaimName], Is.EqualTo(user.IsAdmin.ToString()));
        Assert.That(jwtToken.Payload["customInstruction"], Is.EqualTo(user.CustomInstruction));
    }

    [Test]
    public void GenerateToken_ValidUser_CorrectExpiration()
    {
        var user = new User
        {
            Id = 1,
            Username = "test_user",
            CustomInstruction = "custom_instruction",
            Role = new Role
            {
                Id = 1,
                Name = Roles.Admin
            }
        };

        var token = TokenGenerator.GenerateToken(user, Configuration);
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(token);

        Assert.IsNotNull(jwtToken);

        var expiration = jwtToken.ValidTo;
        var expectedExpiration = DateTime.UtcNow.AddMinutes(60);
        Assert.IsTrue(expiration >= expectedExpiration.AddMinutes(-1) && expiration <= expectedExpiration.AddMinutes(1));
    }
}