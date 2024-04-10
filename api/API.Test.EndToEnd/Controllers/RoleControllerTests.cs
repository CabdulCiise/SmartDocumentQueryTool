using System.Net.Http.Headers;
using System.Net;
using API.Test.EndToEnd.Controllers;

namespace API.Test.EndToEndTest.Controllers;

public class RoleControllerTests : BaseControllerTests
{
    [Test]
    public async Task GetRoles_ReturnsSuccessForAdminUser()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"/role");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtAdminToken);
        var response = await client.SendAsync(request);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task GetRoles_ReturnsUnauthorizedForRegularUser()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"/role");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtUserToken);
        var response = await client.SendAsync(request);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
    }

    [Test]
    public async Task GetChats_ReturnsUnauthorizedForInvalidToken()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"/role");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", invalidJwtToken);
        var response = await client.SendAsync(request);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }
}
