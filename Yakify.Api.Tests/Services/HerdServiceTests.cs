using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;
using Yakify.Api.Models;
using Yakify.Api.Services;
using Yakify.Domain;
using Yakify.Repository;

namespace Yakify.Api.Tests.Services;

public class HerdServiceTests(ITestOutputHelper testOutput) : ServiceTests(testOutput)
{
    [Fact]
    public async Task Herd_can_be_loaded()
    {
        await RunInTransaction(async svp =>
        {
            var service = svp.GetRequiredService<HerdService>();
            await service.LoadNewHerd(Herd(
                ("Yak-1", 1, Sex.Female),
                ("Yak-2", 2.34, Sex.Male)
            ), CancellationToken.None);
        });
        await RunInTransaction(async svp =>
        {
            var repository = svp.GetRequiredService<IYakRepository>();
            var yaks = await repository.GetAll(CancellationToken.None);
            yaks.Should().HaveCount(2);
            var yak1 = Array.Find(yaks, yak => yak.Name == "Yak-1");
            yak1.Should().NotBeNull();
            yak1!.AgeInDays.Should().Be(100);
            yak1.Sex.Should().Be(Sex.Female);
            var yak2 = Array.Find(yaks, yak => yak.Name == "Yak-2");
            yak2.Should().NotBeNull();
            yak2!.AgeInDays.Should().Be(234);
            yak2.Sex.Should().Be(Sex.Male);
        });
    }

    [Fact]
    public async Task Loading_new_herd_overrides_the_old_herd()
    {
        await RunInTransaction(async svp =>
        {
            var service = svp.GetRequiredService<HerdService>();
            await service.LoadNewHerd(Herd(
                ("Yak-1", 1, Sex.Female),
                ("Yak-2", 2, Sex.Male)
            ), CancellationToken.None);
        });
        await RunInTransaction(async svp =>
        {
            var service = svp.GetRequiredService<HerdService>();
            await service.LoadNewHerd(Herd(
                ("Yak-3", 3, Sex.Female),
                ("Yak-4", 4, Sex.Male)
            ), CancellationToken.None);
        });
        await RunInTransaction(async svp =>
        {
            var repository = svp.GetRequiredService<IYakRepository>();
            var yaks = await repository.GetAll(CancellationToken.None);
            yaks.Should().HaveCount(2);
            var yak1 = Array.Find(yaks, yak => yak.Name == "Yak-1");
            yak1.Should().BeNull();
            var yak2 = Array.Find(yaks, yak => yak.Name == "Yak-2");
            yak2.Should().BeNull();
            var yak3 = Array.Find(yaks, yak => yak.Name == "Yak-3");
            yak3.Should().NotBeNull();
            var yak4 = Array.Find(yaks, yak => yak.Name == "Yak-4");
            yak4.Should().NotBeNull();
        });
    }

    private CreateHerdDto Herd(params (string Name, double Age, Sex Sex)[] herd)
    {
        return new CreateHerdDto(herd.Select(yak => new CreateYakDto
        {
            Name = yak.Name,
            Age = yak.Age,
            Sex = yak.Sex,
        }).ToArray());
    }
}
