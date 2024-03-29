﻿namespace Yakify.Domain;

public class Yak
{
    private Yak() {}

    public Yak(string name, double age, Sex sex)
    {
        ThrowOnInvalidInput(name, age);
        Name = name;
        Sex = sex;
        AgeInDays = age.InDays();
    }

    private static void ThrowOnInvalidInput(string name, double age)
    {
        if(string.IsNullOrWhiteSpace(name)) throw new YakException(Errors.YAK_NAME_CANNOT_BE_EMPTY);
        if(age < 0) throw new YakException(Errors.YAK_AGE_CANNOT_BE_NEGATIVE);
        if(IsDeadOnActualAge(age.InDays())) throw new YakException(Errors.YAK_AGE_BEYOND_LIFE_EXPECTANCY);
    }

    public int Id { get; private set; }
    public string Name { get; private set; } = "";
    public Sex Sex { get; private set; }
    public int AgeInDays { get; private set; }

    public double GetMilkProduceOnDay(int day)
    {
        if(Sex == Sex.Male || HasDied(day)) return 0;
        return 50.0 - ActualAgeInDaysAfterDay(day) * 0.03;
    }

    public double TotalMilkProduceUpToAndIncludingDay(int day)
    {
        return Enumerable.Range(0, day + 1).Select(GetMilkProduceOnDay).Sum();
    }

    public bool HasDied(int day) => IsDeadOnActualAge(ActualAgeInDaysAfterDay(day));

    private static bool IsDeadOnActualAge(int ageInDays)
    {
        return ageInDays >= (YAK_LIFE_IN_YEARS * YAK_YEAR_IN_DAYS);
    }

    public int ActualAgeInDaysAfterDay(int day) => day + AgeInDays;
    public double ActualAgeInYearsAfterDay(int day) => ActualAgeInDaysAfterDay(day).InYears();

    public bool NeedsToBeShaved(int day)
    {
        return GetShavingScheduleUpToAndIncluding(day).Contains(day);
    }

    public double? AgeLastShavedInYears(int day)
    {
        var dayLastShaved = GetShavingScheduleUpToAndIncluding(day).Cast<int?>().LastOrDefault();
        if(dayLastShaved == null) return null;
        return ActualAgeInYearsAfterDay(dayLastShaved.Value);
    }

    public int TotalAmountOfHidesProducedUpToAndInclusingDay(int day)
    {
        return GetShavingScheduleUpToAndIncluding(day).Count();
    }

    private IEnumerable<int> GetShavingScheduleUpToAndIncluding(int dayOfMeasurement)
    {
        int day = DayOfFirstShave();
        while(!HasDied(day) && day <= dayOfMeasurement)
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

    internal const int YAK_YEAR_IN_DAYS = 100;
    public const int YAK_LIFE_IN_YEARS = 10;
    private const int AGE_OF_FIRST_SHAVE_IN_YEARS = 1;
}
