using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Xunit.Abstractions;

namespace Yakify.TestBase;

public static class LoggingExtensions
{
    public static void AddSerilogTestLogger(this IServiceCollection services, ITestOutputHelper testOutput)
    {
        services.AddLogging(config => config.AddSerilog(
            new LoggerConfiguration()
                .WriteTo.TestOutput(testOutput)
                .CreateLogger()
        ));
    }
}