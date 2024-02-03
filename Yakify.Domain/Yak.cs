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
        AgeInDays = (int)(100 * age);
    }

    private static void ThrowOnInvalidInput(string name, double age)
    {
        if(string.IsNullOrWhiteSpace(name)) throw new YakException(Errors.YAK_NAME_CANNOT_BE_EMPTY);
        if(age < 0) throw new YakException(Errors.YAK_AGE_CANNOT_BE_NEGATIVE);
    }

    public required string Name { get; init; }
    public required Sex Sex { get; init; }
    public required int AgeInDays { get; init; }
}
