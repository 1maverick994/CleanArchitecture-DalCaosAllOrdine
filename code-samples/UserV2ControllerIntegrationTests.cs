using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

public class UserControllerIntegrationTests : IClassFixture<WebApplicationFactory<Startup>>
{
    private readonly HttpClient _client;

    public UserControllerIntegrationTests(WebApplicationFactory<Startup> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task InsertUser_ValidInput_ReturnsSuccess()
    {
        // Arrange
        var userJson = new StringContent(JsonConvert.SerializeObject(new
        {
            Name = "John",
            Surname = "Doe",
            Email = "john.doe@example.com"
        }), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/users", userJson);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        Assert.Contains("John", responseString);
    }

    [Fact]
    public async Task InsertUser_MissingName_ReturnsBadRequest()
    {
        // Arrange
        var userJson = new StringContent(JsonConvert.SerializeObject(new
        {
            Name = "",
            Surname = "Doe",
            Email = "john.doe@example.com"
        }), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/users", userJson);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
