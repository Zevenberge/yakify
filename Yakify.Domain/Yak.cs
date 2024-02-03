using System.Diagnostics.CodeAnalysis;

namespace Yakify.Domain;

public class Yak
{
    private Yak() {}

    [SetsRequiredMembers]
    public Yak(string name, double age, Sex sex)
    {
        ThrowOnInvalidInput(name, age);
        Name = name;
        Sex = sex;
        AgeInDays = ConvertAgeFromYearsToDays(age);
    }

    private static void ThrowOnInvalidInput(string name, double age)
    {
        if(string.IsNullOrWhiteSpace(name)) throw new YakException(Errors.YAK_NAME_CANNOT_BE_EMPTY);
        if(age < 0) throw new YakException(Errors.YAK_AGE_CANNOT_BE_NEGATIVE);
        if(IsDeadOnActualAge(ConvertAgeFromYearsToDays(age))) throw new YakException(Errors.YAK_AGE_BEYOND_LIFE_EXPECTANCY);
    }

    private static int ConvertAgeFromYearsToDays(double ageInYears) => (int)(YAK_YEAR_IN_DAYS * ageInYears);

    public required string Name { get; init; }
    public required Sex Sex { get; init; }
    public required int AgeInDays { get; init; }

    public double GetMilkProduceOnDay(int day)
    {
        if(Sex == Sex.Male || HasDied(day)) return 0;
        return 50.0 - ActualAgeInDaysAfterDay(day) * 0.03;
    }

    public bool HasDied(int day) => IsDeadOnActualAge(ActualAgeInDaysAfterDay(day));

    private static bool IsDeadOnActualAge(int ageInDays)
    {
        return ageInDays >= (YAK_LIFE_IN_YEARS * YAK_YEAR_IN_DAYS);
    }

    public int ActualAgeInDaysAfterDay(int day) => day + AgeInDays;

    public bool NeedsToBeShaved(int day)
    {
        return GetShavingSchedule().Contains(day);
    }

    private IEnumerable<int> GetShavingSchedule()
    {
        int day = DayOfFirstShave();
        while(!HasDied(day))
        {
            yield return day;
            day += ShavingInterval(day);
        }
    }

    private int DayOfFirstShave()
    {
        const int dayEligableForShaving = AGE_OF_FIRST_SHAVE_IN_YEARS * YAK_YEAR_IN_DAYS;
        return Math.Max(0, dayEligableForShaving - AgeInDays);
    }

    private int ShavingInterval(int dayOfShaving)
    {
        return 9 + ActualAgeInDaysAfterDay(dayOfShaving) / 100;
    }

    private const int YAK_YEAR_IN_DAYS = 100;
    private const int YAK_LIFE_IN_YEARS = 10;
    private const int AGE_OF_FIRST_SHAVE_IN_YEARS = 1;
}
