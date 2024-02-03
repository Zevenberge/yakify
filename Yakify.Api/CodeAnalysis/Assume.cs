using System.Diagnostics.CodeAnalysis;

namespace Yakify.Api.CodeAnalysis;

public static class Assume
{
    public static void NotNull<T>([NotNull]T? value)
    {
        if(value == null) 
        {
            throw new ArgumentNullException(nameof(value));
        }
    }
}