using System.Diagnostics.CodeAnalysis;

namespace Yakify.Domain;

public class Yak
{
    private Yak() {}

    [SetsRequiredMembers]
    public Yak(string name, double age, Sex sex)
    {
        Name = name;
        Sex = sex;
        AgeInDays = (int)(100 * age);
    }

    public required string Name { get; init; }
    public required Sex Sex { get; init; }
    public required int AgeInDays { get; init; }
}
