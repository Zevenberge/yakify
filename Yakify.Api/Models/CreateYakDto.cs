using System.ComponentModel.DataAnnotations;
using Yakify.Domain;

namespace Yakify.Api.Models;

public class CreateYakDto
{
    [Required(ErrorMessage = Errors.YAK_NAME_CANNOT_BE_EMPTY)]
    public string? Name { get; set; }

    [Required(ErrorMessage = Errors.YAK_AGE_CANNOT_BE_EMPTY)]
    [Min(0, ErrorMessage = Errors.YAK_AGE_CANNOT_BE_NEGATIVE)]
    [Max(Yak.YAK_LIFE_IN_YEARS, MaximumIsExclusive = true, ErrorMessage = Errors.YAK_AGE_BEYOND_LIFE_EXPECTANCY)]
    public double? Age { get; set; }

    [Required(ErrorMessage = Errors.YAK_SEX_CANNOT_BE_EMPTY)]
    [AllowedValues(Domain.Sex.Female, Domain.Sex.Male, ErrorMessage = Errors.YAK_SEX_INVALID_VALUE)]
    public Sex? Sex { get; set; }
}
