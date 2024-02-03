using Yakify.Api.Models;
using Yakify.Domain;

namespace Yakify.Api.Tests.Models;

public class CreateYakDtoTests
{
    [Fact]
    public void Yak_is_valid()
    {
        var dto = new CreateYakDto
        {
            Name = "Yak-1",
            Age = 4,
            Sex = Sex.Female,
        };
        dto.ShouldBeValid();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Yak_without_name_is_not_valid(string? name)
    {
        var dto = new CreateYakDto
        {
            Name = name,
            Age = 4,
            Sex = Sex.Female,
        };
        dto.ShouldHaveError(nameof(CreateYakDto.Name), Errors.YAK_NAME_CANNOT_BE_EMPTY);
    }

    [Fact]
    public void Yak_without_age_is_not_valid()
    {
        var dto = new CreateYakDto
        {
            Name = "Yak-1",
            Age = null,
            Sex = Sex.Female,
        };
        dto.ShouldHaveError(nameof(CreateYakDto.Age), Errors.YAK_AGE_CANNOT_BE_EMPTY);
    }

    [Fact]
    public void Newly_born_yak_is_valid()
    {
        var dto = new CreateYakDto
        {
            Name = "Yak-New",
            Age = 0,
            Sex = Sex.Female,
        };
        dto.ShouldBeValid();
    }

    [Fact]
    public void Yak_with_negative_age_is_not_valid()
    {
        var dto = new CreateYakDto
        {
            Name = "Yak-1",
            Age = -0.001,
            Sex = Sex.Female,
        };
        dto.ShouldHaveError(nameof(CreateYakDto.Age), Errors.YAK_AGE_CANNOT_BE_NEGATIVE);
    }

    [Fact]
    public void Yak_with_mortal_age_is_not_valid()
    {
        var dto = new CreateYakDto
        {
            Name = "Yak-1",
            Age = 10,
            Sex = Sex.Female,
        };
        dto.ShouldHaveError(nameof(CreateYakDto.Age), Errors.YAK_AGE_BEYOND_LIFE_EXPECTANCY);
    }

    [Fact]
    public void Yak_without_sex_is_not_valid()
    {
        var dto = new CreateYakDto
        {
            Name = "Yak-1",
            Age = 10,
            Sex = null,
        };
        dto.ShouldHaveError(nameof(CreateYakDto.Sex), Errors.YAK_SEX_CANNOT_BE_EMPTY);
    }

    [Fact]
    public void Yak_invalid_sex_is_not_valid()
    {
        var dto = new CreateYakDto
        {
            Name = "Yak-1",
            Age = 10,
            Sex = (Sex)666,
        };
        dto.ShouldHaveError(nameof(CreateYakDto.Sex), Errors.YAK_SEX_INVALID_VALUE);
    }
}