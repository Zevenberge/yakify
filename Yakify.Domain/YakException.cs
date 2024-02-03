namespace Yakify.Domain;

public class YakException : Exception
{
    public YakException(string? message) : base(message)
    {
    }
}