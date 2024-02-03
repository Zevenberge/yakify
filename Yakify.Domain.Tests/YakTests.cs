namespace Yakify.Domain.Tests;

public class YakTests
{
    [Theory]
    [InlineData("Yak-1", 0, Sex.Female, 0)]
    [InlineData("Yak-2", 4, Sex.Male, 400)]
    public void Yak_is_initialized_correctly(string name, int ageInYears, Sex sex, int ageInDays)
    {
        var yak = new Yak(name, ageInYears, sex);
        yak.Name.Should().Be(name);
        yak.AgeInDays.Should().Be(ageInDays);
        yak.Sex.Should().Be(sex);
    }
}