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

    [Fact]
    public void Dead_yak_cannot_be_created()
    {
        // In this world, yaks physically don't age beyond 10. Otherwise
        // the app would be telling you that the yak next you couldn't
        // possibly be there.
        FluentActions.Invoking(() => new Yak("Zombie-Yak", 10, Sex.Female))
            .Should().Throw<YakException>()
                .WithMessage(Errors.YAK_AGE_BEYOND_LIFE_EXPECTANCY);
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
    [InlineData(2.25, 75, 300)]
    [InlineData(1, 15, 115)]
    public void Yak_ages_over_time(double initialAgeInYears, int day, int actualAge)
    {
        var yak = new Yak("Growing-Yak", initialAgeInYears, Sex.Female);
        yak.ActualAgeInDaysAfterDay(day).Should().Be(actualAge);
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

    [Fact]
    public void Male_yaks_dont_produce_milk()
    {
        var yak = new Yak("Bulky-Yak", 2, Sex.Male);
        yak.GetMilkProduceOnDay(0).Should().Be(0);
        yak.GetMilkProduceOnDay(10).Should().Be(0);
        yak.GetMilkProduceOnDay(100).Should().Be(0);
    }

    [Fact]
    public void Dead_yaks_dont_produce_milk()
    {
        var yak = new Yak("Milk-Yak", 0, Sex.Female);
        yak.GetMilkProduceOnDay(1_000).Should().Be(0);
    }

    [Fact]
    public void Yak_dies_the_day_they_age_10_in_years()
    {
        var yak = new Yak("Old-Yak", 0, Sex.Female);
        yak.HasDied(999).Should().BeFalse();
        yak.HasDied(1_000).Should().BeTrue();
        yak.HasDied(1_001).Should().BeTrue();
    }

    [Fact]
    public void Yak_dies_the_day_they_age_10_in_years_accounting_for_their_initial_age()
    {
        var yak = new Yak("Old-Yak", 5, Sex.Female);
        yak.HasDied(499).Should().BeFalse();
        yak.HasDied(500).Should().BeTrue();
    }

    [Fact]
    public void Yak_needs_to_be_shaved_on_day_0()
    {
        var yak = new Yak("Hairy-Yak", 1, Sex.Male);
        yak.NeedsToBeShaved(0).Should().BeTrue();
    }

    [Theory]
    [InlineData(Sex.Female)]
    [InlineData(Sex.Male)]
    public void Both_sexes_need_to_be_shaved(Sex sex)
    {
        var yak = new Yak("Hairy-Yak", 1, sex);
        yak.NeedsToBeShaved(0).Should().BeTrue();
    }
}