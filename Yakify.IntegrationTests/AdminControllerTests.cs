using System.Net;
using FluentAssertions;
using Yakify.Api;

namespace Yakify.IntegrationTests;

public class AdminControllerTests(YakifyWebApplicationFactory<Program> factory) : IntegrationTest(factory)
{
    [Fact]
    public async Task Post_herd_205_Reset_content()
    {
        var client = Factory.CreateClient();
        var response = await client.PostAsJsonAsync("/yak-shop/load", new
        {
            Herd = new[] {
                new {
                    Name = "Yak-1",
                    Age = 4,
                    Sex = "FEMALE"
                }
            }
        });
        response.StatusCode.Should().Be(HttpStatusCode.ResetContent);
    }
}
