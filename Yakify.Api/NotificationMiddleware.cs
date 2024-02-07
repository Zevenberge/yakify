using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.AspNetCore.SignalR;
using Yakify.Api.CodeAnalysis;

namespace Yakify.Api;

public static class NotificationMiddleware
{
    public static void UseNotifications(this WebApplication app)
    {
        app.Use(async (context, next) =>
        {
            await next();
            if (context.ShouldUseNotification(out var attribute))
            {
                var clients = GetHubClients(context.RequestServices, attribute);
                await clients.All.SendAsync("changed");
            }
        });
    }

    private static bool ShouldUseNotification(this HttpContext context, [NotNullWhen(true)] out NotificationAttribute? attribute)
    {
        attribute = context.GetEndpoint()?.Metadata.OfType<NotificationAttribute>().FirstOrDefault();
        return attribute != null;
    }

    private static IHubClients GetHubClients(IServiceProvider serviceProvider, NotificationAttribute attribute)
    {
#pragma warning disable S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields
        var method = typeof(NotificationMiddleware).GetMethod(nameof(GetHubClientsImpl), BindingFlags.Static | BindingFlags.NonPublic);
#pragma warning restore S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields
        Assume.NotNull(method);
        return (IHubClients)method.MakeGenericMethod(attribute.HubType).Invoke(null, [serviceProvider])!;
    }

    private static IHubClients GetHubClientsImpl<THub>(IServiceProvider serviceProvider)
        where THub : Hub
    {
        return serviceProvider.GetRequiredService<IHubContext<THub>>().Clients;
    }
}
