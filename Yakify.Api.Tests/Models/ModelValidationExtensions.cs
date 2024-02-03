using System.ComponentModel.DataAnnotations;

namespace Yakify.Api.Tests.Models;

public static class ModelValidationExtensions
{
    public static void ShouldBeValid<T>(this T model) where T : notnull
    {
        Validate(model).Should().BeEmpty();
    }

    public static void ShouldHaveError<T>(this T model, string property, string errorMessage) where T : notnull
    {
        var errors = Validate(model).ToList();
        errors.Should().NotBeEmpty();
        var errorProperty = errors.Find(e => e.MemberNames.Any(n => n == property));
        errorProperty.Should().NotBeNull();
        errorProperty!.ErrorMessage.Should().Be(errorMessage);
    }

    public static IEnumerable<ValidationResult> Validate<T>(T model) where T : notnull
    {
        var result = new List<ValidationResult>();
        if (Validator.TryValidateObject(model, new ValidationContext(model), result, true))
        {
            return Array.Empty<ValidationResult>();
        }
        return result;
    }
}