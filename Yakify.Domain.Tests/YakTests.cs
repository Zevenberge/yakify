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
    [InlineData(2.25, 75, 3.00)]
    [InlineData(1, 15, 1.15)]
    public void Yak_age_can_be_calculated_in_years(double initialAgeInYears, int day, double actualAge)
    {
        var yak = new Yak("Growing-Yak", initialAgeInYears, Sex.Female);
        yak.ActualAgeInYearsAfterDay(day).Should().Be(actualAge);
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

    [Fact]
    public void Yak_of_less_than_a_year_old_does_not_need_to_be_shaved()
    {
        var yak = new Yak("Young-Yak", 0.99, Sex.Male);
        yak.NeedsToBeShaved(0).Should().BeFalse();
    }

    [Fact]
    public void As_soon_as_a_yak_turns_1_it_needs_to_be_shaved()
    {
        var yak = new Yak("Young-Yak", 0.99, Sex.Male);
        yak.NeedsToBeShaved(1).Should().BeTrue();
    }

    [Theory]
    [InlineData(Sex.Female)]
    [InlineData(Sex.Male)]
    public void Both_sexes_need_to_be_shaved(Sex sex)
    {
        var yak = new Yak("Hairy-Yak", 1, sex);
        yak.NeedsToBeShaved(0).Should().BeTrue();
    }

    [Fact]
    public void After_a_yak_it_shaved_it_cannot_be_shaved_until_its_hair_regrows()
    {
        var yak = new Yak("Hairy-Yak", 1, Sex.Male);
        yak.NeedsToBeShaved(1).Should().BeFalse();
    }

    [Fact]
    public void After_the_hair_regrows_the_yak_can_be_shaved_again()
    {
        var yak = new Yak("Hairy-Yak", 1, Sex.Male);
        yak.NeedsToBeShaved(10).Should().BeTrue();
        yak.NeedsToBeShaved(11).Should().BeFalse();
    }

    [Theory]
    [InlineData(1, 10)]
    [InlineData(2, 11)]
    [InlineData(3, 12)]
    [InlineData(4, 13)]
    public void Time_for_hair_to_regrow_is_dependent_on_age(int age, int dayOfSecondShave)
    {
        var yak = new Yak("Hairy-Yak", age, Sex.Male);
        yak.NeedsToBeShaved(dayOfSecondShave - 1).Should().BeFalse();
        yak.NeedsToBeShaved(dayOfSecondShave).Should().BeTrue();
        yak.NeedsToBeShaved(dayOfSecondShave + 1).Should().BeFalse();
    }

    [Fact]
    public void Dead_yaks_will_not_be_shaved()
    {
        var yak = new Yak("Last-Yak", 9.82, Sex.Male);
        yak.NeedsToBeShaved(18).Should().BeFalse();
    }

    [Fact]
    public void Age_last_shaved_is_null_when_never_shaved()
    {
        var yak = new Yak("Bald-Yak", 0, Sex.Male);
        yak.AgeLastShavedInYears(0).Should().BeNull();
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(2.0)]
    [InlineData(3.0)]
    public void Age_last_shaved_is_its_inital_age_when_shaved_on_first_day(double ageInYears)
    {
        var yak = new Yak("Hairy-Yak", ageInYears, Sex.Male);
        yak.AgeLastShavedInYears(0).Should().Be(ageInYears);
    }

    [Theory]
    [InlineData(1.0, 2, 1.0)]
    [InlineData(1.0, 11, 1.10)]
    [InlineData(2.0, 11, 2.11)]
    public void Age_last_shaved_returns_the_age_in_years_at_the_day_of_shaving(double ageInYears, int day, double ageLastShavedInYears)
    {
        var yak = new Yak("Hairy-Yak", ageInYears, Sex.Male);
        yak.AgeLastShavedInYears(day).Should().Be(ageLastShavedInYears);
    }

    [Fact]
    public void Age_last_shaved_returns_the_age_of_first_shave()
    {
        var yak = new Yak("Adolescent-Yak", 0.9, Sex.Male);
        yak.AgeLastShavedInYears(15).Should().Be(1.0);
    }

    [Fact]
    public void Age_last_shaved_of_dead_yaks_returns_the_shave_when_they_were_still_alive()
    {
        var yak = new Yak("Old-Yak", 9.5, Sex.Male);
        yak.AgeLastShavedInYears(100).Should().Be(9.86);
    }

    [Theory]
    [InlineData(2.0, 44.0)]
    [InlineData(3.0, 41.0)]
    [InlineData(4.0, 38.0)]
    public void Total_milk_produce_includes_the_day_itself(double ageInYears, double milkProduce)
    {
        var yak = new Yak("Milk-Yak", ageInYears, Sex.Female);
        yak.TotalMilkProduceUpToAndIncludingDay(0).Should().Be(milkProduce);
    }

    [Theory]
    [InlineData(0, 44.00)]
    [InlineData(1, 87.97)]
    [InlineData(2, 131.91)]
    [InlineData(3, 175.82)]
    public void Total_milk_produce_is_total_of_days_before(int day, double totalMilk)
    {
        var yak = new Yak("Milk-Yak", 2.0, Sex.Female);
        yak.TotalMilkProduceUpToAndIncludingDay(day).Should().Be(totalMilk);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    [InlineData(2, 1)]
    [InlineData(13, 2)]
    public void Total_hide_produce_is_amount_of_times_shaved(int day, int hides)
    {
        var yak = new Yak("Hide-Yak", 0.99, Sex.Female);
        yak.TotalAmountOfHidesProducedUpToAndInclusingDay(day).Should().Be(hides);
    }
}
