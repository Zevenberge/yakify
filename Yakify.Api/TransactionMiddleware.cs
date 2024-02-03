using Yakify.Repository;

namespace Yakify.Api;

public static class TransactionMiddleware
{
    public static void UseTransaction(this WebApplication app)
    {
        app.Use(async (context, next) => {
            await next();
            if(IsSuccessStatusCode(context.Response.StatusCode))
            {
                await context.RequestServices.GetRequiredService<IUnitOfWork>().SaveChangesAsync(context.RequestAborted);
            }
        });
    }

    private static bool IsSuccessStatusCode(int statusCode)
    {
        return statusCode >= 200 && statusCode < 300;
    }
}