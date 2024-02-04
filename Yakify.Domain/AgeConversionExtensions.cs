namespace Yakify.Domain;

internal static class AgeConversionExtensions
{
    public static int InDays(this double ageInYears)
    {
        return (int)(ageInYears * Yak.YAK_YEAR_IN_DAYS);
    }

    public static double InYears(this int ageInDays)
    {
        return ((double)ageInDays) / Yak.YAK_YEAR_IN_DAYS;
    }
}