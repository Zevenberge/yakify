using Xunit.Abstractions;
using Yakify.Domain;

namespace Yakify.Repository.Tests;

public class YakRepositoryTests(ITestOutputHelper testOutput) : RepositoryTests(testOutput)
{
    [Fact]
    public async Task Yaks_can_be_added_and_retreived()
    {
        var yak1 = new Yak("Yak-1", 1, Sex.Female);
        var yak2 = new Yak("Yak-2", 2, Sex.Male);
        await RunInTransaction<IYakRepository>(async repository =>
        {
            await repository.AddRange([yak1, yak2], CancellationToken.None);
        });
        await RunInTransaction<IYakRepository>(async repository => 
        {
            var yaks = await repository.GetAll(CancellationToken.None);
            yaks.Should().HaveCount(2);
            var yak1FromDatabase = Array.Find(yaks, y => y.Name == "Yak-1");
            yak1FromDatabase.Should().NotBeNull();
            yak1FromDatabase!.AgeInDays.Should().Be(100);
            yak1FromDatabase.Sex.Should().Be(Sex.Female);
        });
    }

    [Fact]
    public async Task Yaks_can_be_deleted()
    {
         var yak1 = new Yak("Yak-1", 1, Sex.Female);
        var yak2 = new Yak("Yak-2", 2, Sex.Male);
        await RunInTransaction<IYakRepository>(async repository =>
        {
            await repository.AddRange([yak1, yak2], CancellationToken.None);
        });
        await RunInTransaction<IYakRepository>(async repository => 
        {
            await repository.DeleteAll(CancellationToken.None);
        });
        await RunInTransaction<IYakRepository>(async repository => 
        {
            var yaks = await repository.GetAll(CancellationToken.None);
            yaks.Should().HaveCount(0);
        });
    }
}
