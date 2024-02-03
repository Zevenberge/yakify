using Yakify.Api;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Configure();
await app.RunMigrations();
await app.RunAsync();