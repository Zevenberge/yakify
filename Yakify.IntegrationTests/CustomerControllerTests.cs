using System.Net;
using FluentAssertions;
using Yakify.Api;
using Yakify.Api.Models;

namespace Yakify.IntegrationTests;

public class CustomerControllerTests(YakifyWebApplicationFactory<Program> factory) : IntegrationTest(factory)
{
    [Fact]
    public async Task Order_200_Ok()
    {
        await CreateYak("Yak", 1, "FEMALE");
        var client = Factory.CreateClient();
        var response = await client.PostAsJsonAsync("/yak-shop/order/1", new
        {
            Customer = "Garry",
            Order = new {
                Milk = 10.0,
                Skins = 1,
            },
        });
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var body = await response.Content.ReadFromJsonAsync<OrderDto>();
        body.Should().NotBeNull();
        body!.Milk.Should().Be(10);
        body.Skins.Should().Be(1);
    }

    [Theory]
    [InlineData(100, 1, null, 1)]
    [InlineData(10.0, 5, 10.0, null)]
    public async Task Order_206_Partial_content(double milk, int skins, double? milkExpected, int? skinsExpected)
    {
        await CreateYak("Yak", 1, "FEMALE");
        var client = Factory.CreateClient();
        var response = await client.PostAsJsonAsync("/yak-shop/order/1", new
        {
            Customer = "Garry",
            Order = new {
                Milk = milk,
                Skins = skins,
            },
        });
        response.StatusCode.Should().Be(HttpStatusCode.PartialContent);
        var body = await response.Content.ReadFromJsonAsync<OrderDto>();
        body.Should().NotBeNull();
        body!.Milk.Should().Be(milkExpected);
        body.Skins.Should().Be(skinsExpected);
    }

    [Fact]
    public async Task Order_404_Not_found()
    {
        await CreateYak("Yak", 1, "FEMALE");
        var client = Factory.CreateClient();
        var response = await client.PostAsJsonAsync("/yak-shop/order/1", new
        {
            Customer = "Garry",
            Order = new {
                Milk = 1000,
                Skins = 10,
            },
        });
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}