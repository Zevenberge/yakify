using Yakify.Api.CodeAnalysis;

namespace Yakify.Api.Tests.CodeAnalysis;

public class AssumeTests
{
    [Fact]
    public void Assume_not_null_does_not_throw_if_not_null()
    {
        FluentActions.Invoking(() => Assume.NotNull("123"))
            .Should().NotThrow();
    }

    [Fact]
    public void Assume_not_null_throws_if_null()
    {
        FluentActions.Invoking(() => Assume.NotNull((string?)null))
            .Should().Throw<ArgumentNullException>();
    }
}