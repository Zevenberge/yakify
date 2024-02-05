namespace Yakify.Api;

public class Program
{
    protected Program()
    {
    }

    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Configure();
        await app.RunMigrations();
        await app.RunAsync();
    }
}