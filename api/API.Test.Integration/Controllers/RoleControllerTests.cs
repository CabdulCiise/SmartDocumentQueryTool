using API.Core.Models;
using API.Core.Services;
using API.Web.Controllers;
using Moq;

namespace API.Test.Integration.Controllers;

[TestFixture]
public class RoleControllerTests
{
    private RoleController roleController;
    private Mock<IRoleService> mockRoleService;

    public RoleControllerTests()
    {
        mockRoleService = new Mock<IRoleService>();
        roleController = new RoleController(mockRoleService.Object);
    }

    [Test]
    public void GetRoles_CallGetRolesInService()
    {
        var expectedRoles = new List<Role>();
        mockRoleService.Setup(x => x.GetRoles()).Returns(expectedRoles);

        var result = roleController.GetRoles();

        mockRoleService.Verify(x => x.GetRoles(), Times.Once);

        Assert.IsNotNull(result);
        Assert.IsInstanceOf<List<Role>>(result);
        Assert.That(result, Is.EqualTo(expectedRoles));
    }
}
