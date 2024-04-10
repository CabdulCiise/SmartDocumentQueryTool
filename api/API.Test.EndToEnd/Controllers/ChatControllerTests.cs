using API.Test.EndToEnd.Controllers;
using System.Net;
using System.Net.Http.Headers;

namespace API.Test.EndToEndTest.Controllers;

public class ChatControllerTests : BaseControllerTests
{
    [Test]
    public async Task GetResponse_ReturnsSuccessForRegularUser()
    {
        int chatId = 1;
        int userId = 1;
        string prompt = "Test prompt";

        var request = new HttpRequestMessage(HttpMethod.Get, $"/chat/{chatId}/user/{userId}/{prompt}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtUserToken);
        var response = await client.SendAsync(request);

        Assert.That(response.IsSuccessStatusCode, Is.True);
    }

    [Test]
    public async Task GetResponse_ReturnsUnauthorizedForInvalidToken()
    {
        int chatId = 1;
        int userId = 1;
        string prompt = "Test prompt";

        var request = new HttpRequestMessage(HttpMethod.Get, $"/chat/{chatId}/user/{userId}/{prompt}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", invalidJwtToken);
        var response = await client.SendAsync(request);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task GetChats_ReturnsSuccessForRegularUser()
    {
        int userId = 2;

        var request = new HttpRequestMessage(HttpMethod.Get, $"/chat/user/{userId}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtUserToken);
        var response = await client.SendAsync(request);

        Assert.That(response.IsSuccessStatusCode, Is.True);
    }

    [Test]
    public async Task GetChats_ReturnsUnauthorizedForInvalidToken()
    {
        int userId = 2;

        var request = new HttpRequestMessage(HttpMethod.Get, $"/chat/user/{userId}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", invalidJwtToken);
        var response = await client.SendAsync(request);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task GetChatHistory_ReturnsSuccessForRegularUser()
    {
        int chatId = 1;

        var request = new HttpRequestMessage(HttpMethod.Get, $"/chat/{chatId}/messages");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtUserToken);
        var response = await client.SendAsync(request);

        Assert.That(response.IsSuccessStatusCode, Is.True);
    }

    [Test]
    public async Task GetChatHistory_ReturnsUnauthorizedForInvalidToken()
    {
        int chatId = 1;

        var request = new HttpRequestMessage(HttpMethod.Get, $"/chat/{chatId}/messages");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", invalidJwtToken);
        var response = await client.SendAsync(request);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task DeleteChat_ReturnsSuccessForRegularUser()
    {
        int chatId = 1;

        var request = new HttpRequestMessage(HttpMethod.Delete, $"/chat/{chatId}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtUserToken);
        var response = await client.SendAsync(request);

        Assert.That(response.IsSuccessStatusCode, Is.True);
    }

    [Test]
    public async Task DeleteChat_ReturnsUnauthorizedForInvalidToken()
    {
        int chatId = 1;

        var request = new HttpRequestMessage(HttpMethod.Delete, $"/chat/{chatId}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", invalidJwtToken);
        var response = await client.SendAsync(request);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }
}
