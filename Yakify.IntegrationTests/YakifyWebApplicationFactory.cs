using Microsoft.AspNetCore.Mvc.Testing;

namespace Yakify.IntegrationTests;

public class YakifyWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseConfiguration(new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                {"ConnectionString", "Server=localhost,5433;Database=Yakify;User Id=sa;Password=Testing-1-2-Yak;TrustServerCertificate=True"}
            })
            .Build());
        base.ConfigureWebHost(builder);
    }
}