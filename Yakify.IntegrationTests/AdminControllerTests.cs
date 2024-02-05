using System.Net;
using FluentAssertions;
using Yakify.Api;
using Yakify.Api.Models;
using Yakify.Domain;
using Yakify.Repository;

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
        await RunWithScopedService<IYakRepository>(async repository =>
        {
            var yaks = await repository.GetAll(CancellationToken.None);
            yaks.Should().HaveCount(1);
            yaks[0].Name.Should().Be("Yak-1");
            yaks[0].AgeInDays.Should().Be(400);
            yaks[0].Sex.Should().Be(Sex.Female);
        });
    }

    [Theory]
    [InlineData("", 4, "FEMALE")]
    [InlineData("Yak", -1, "FEMALE")]
    [InlineData("Yak", 4, "MAIL")]
    public async Task Post_herd_400_Bad_request(string name, double age, string sex)
    {
        var client = Factory.CreateClient();
        var response = await client.PostAsJsonAsync("/yak-shop/load", new
        {
            Herd = new[] {
                new {
                    Name = name,
                    Age = age,
                    Sex = sex
                }
            }
        });
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        await RunWithScopedService<IYakRepository>(async repository =>
        {
            var yaks = await repository.GetAll(CancellationToken.None);
            yaks.Should().HaveCount(0);
        });
    }

    [Fact]
    public async Task Get_herd_status_200_Ok()
    {
        await CreateYak("Yak", 1, "FEMALE");
        var client = Factory.CreateClient();
        var response = await client.GetAsync("/yak-shop/herd/10");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<HerdStatusDto>();
        body.Should().NotBeNull();
        body!.Herd.Should().HaveCount(1);
        body.Herd[0].Name.Should().Be("Yak");
        body.Herd[0].Age.Should().Be(1.1);
        body.Herd[0].AgeLastShaved.Should().Be(1.1);
    }

    [Fact]
    public async Task Get_herd_status_400_Bad_request()
    {
        var client = Factory.CreateClient();
        var response = await client.GetAsync("/yak-shop/herd/-1");
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Get_stock_200_Ok()
    {
        await CreateYak("Stock-Yak", 1, "FEMALE");
        var client = Factory.CreateClient();
        var response = await client.GetAsync("/yak-shop/stock/10");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<StockDto>();
        body.Should().NotBeNull();
        body!.Milk.Should().Be(515.35);
        body.Skins.Should().Be(2);
    }
}
