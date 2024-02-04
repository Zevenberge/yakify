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

    [Fact]
    public async Task Herd_status_shows_age_and_last_shaved()
    {
        await RunInTransaction(async svp =>
        {
            var service = svp.GetRequiredService<HerdService>();
            await service.LoadNewHerd(Herd(
                ("Yak-1", 4, Sex.Female),
                ("Yak-2", 8, Sex.Female),
                ("Yak-3", 9.5, Sex.Female)
            ), CancellationToken.None);
        });
        await RunInTransaction(async svp =>
        {
            var service = svp.GetRequiredService<HerdService>();
            var status = await service.GetHerdStatus(13, CancellationToken.None);
            status.Herd.Should().HaveCount(3);
            status.Herd.Should().BeEquivalentTo([
                new YakStatusDto("Yak-1", 4.13, 4.13),
                new YakStatusDto("Yak-2", 8.13, 8.0),
                new YakStatusDto("Yak-3", 9.63, 9.5),
            ]);
        });
    }

    [Fact]
    public async Task Herd_status_does_not_show_dead_yaks()
    {
        await RunInTransaction(async svp =>
        {
            var service = svp.GetRequiredService<HerdService>();
            await service.LoadNewHerd(Herd(
                ("Yak-1", 4, Sex.Female),
                ("Yak-2", 8, Sex.Female),
                ("Yak-3", 9.5, Sex.Female)
            ), CancellationToken.None);
        });
        await RunInTransaction(async svp =>
        {
            var service = svp.GetRequiredService<HerdService>();
            var status = await service.GetHerdStatus(52, CancellationToken.None);
            status.Herd.Should().HaveCount(2);
            status.Herd.Should().BeEquivalentTo([
                new YakStatusDto("Yak-1", 4.52, 4.52),
                new YakStatusDto("Yak-2", 8.52, 8.51),
            ]);
        });
    }

    [Fact]
    public async Task Herd_status_returns_empty_herd_when_there_is_no_herd_loaded()
    {
        await RunInTransaction(async svp =>
        {
            var service = svp.GetRequiredService<HerdService>();
            var status = await service.GetHerdStatus(0, CancellationToken.None);
            status.Herd.Should().HaveCount(0);
        });
    }

    [Fact]
    public async Task Total_produce_sums_the_produce_of_the_whole_herd()
    {
        await RunInTransaction(async svp =>
        {
            var service = svp.GetRequiredService<HerdService>();
            await service.LoadNewHerd(Herd(
                ("Yak-1", 4, Sex.Female),
                ("Yak-2", 8, Sex.Female),
                ("Yak-3", 9.5, Sex.Female)
            ), CancellationToken.None);
        });
        await RunInTransaction(async svp =>
        {
            var service = svp.GetRequiredService<HerdService>();
            var produce = await service.GetTotalProduce(15, CancellationToken.None);
            produce.Milk.Should().BeApproximately(1357.2, 1E-10);
            produce.Skins.Should().Be(4);
        });
    }

    [Fact]
    public async Task Total_produce_is_0_without_yaks()
    {
        await RunInTransaction(async svp =>
        {
            var service = svp.GetRequiredService<HerdService>();
            var produce = await service.GetTotalProduce(15, CancellationToken.None);
            produce.Milk.Should().Be(0);
            produce.Skins.Should().Be(0);
        });
    }
}
