namespace Yakify.Api;

public class StartupException : Exception
{
    public StartupException(string? message) : base(message)
    {
    }
}