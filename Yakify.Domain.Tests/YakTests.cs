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

    [Theory]
    [InlineData(3.14, 314)]
    [InlineData(2.73546, 273)]
    public void Age_is_rounded_down_to_whole_days(double ageInYears, int ageInDays)
    {
        var yak = new Yak("Test-Yak", ageInYears, Sex.Female);
        yak.AgeInDays.Should().Be(ageInDays);
    }

    [Fact]
    public void Yak_cannot_have_negative_age()
    {
        FluentActions.Invoking(() => new Yak("Baby-Yak", -0.001, Sex.Female))
            .Should().Throw<YakException>()
                .WithMessage(Errors.YAK_AGE_CANNOT_BE_NEGATIVE);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    public void Yak_cannot_have_empty_name(string? name)
    {
        FluentActions.Invoking(() => new Yak(name!, 2, Sex.Female))
            .Should().Throw<YakException>()
                .WithMessage(Errors.YAK_NAME_CANNOT_BE_EMPTY);
    }

    [Theory]
    [InlineData(0, 50.0)]
    [InlineData(1, 49.97)]
    [InlineData(10, 49.7)]
    [InlineData(50, 48.5)]
    [InlineData(999, 20.03)]
    public void Female_yaks_produce_milk_depending_on_age(int day, double litersOfMilk)
    {
        var yak = new Yak("Milk-Yak", 0, Sex.Female);
        yak.GetMilkProduceOnDay(day).Should().Be(litersOfMilk);
    }

    [Theory]
    [InlineData(0, 44.0)]
    [InlineData(1, 43.97)]
    [InlineData(10, 43.7)]
    [InlineData(50, 42.5)]
    [InlineData(799, 20.03)]
    public void Female_yaks_produce_milk_depending_on_age_and_their_initial_age(int day, double litersOfMilk)
    {
        var yak = new Yak("Milk-Yak", 2, Sex.Female);
        yak.GetMilkProduceOnDay(day).Should().Be(litersOfMilk);
    }
}