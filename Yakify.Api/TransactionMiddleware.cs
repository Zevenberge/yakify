using Yakify.Repository;

namespace Yakify.Api;

public static class TransactionMiddleware
{
    public static void UseTransaction(this WebApplication app)
    {
        app.Use(async (context, next) => {
            var dbContext = context.RequestServices.GetRequiredService<YakifyDbContext>();
            using var transaction = await dbContext.Database.BeginTransactionAsync(context.RequestAborted);
            try
            {
                await next();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
            if(IsSuccessStatusCode(context.Response.StatusCode))
            {
                await dbContext.SaveChangesAsync(context.RequestAborted);
                await transaction.CommitAsync(context.RequestAborted);
            }
            else
            {
                await transaction.RollbackAsync();
            }
        });
    }

    private static bool IsSuccessStatusCode(int statusCode)
    {
        return statusCode >= 200 && statusCode < 300;
    }
}