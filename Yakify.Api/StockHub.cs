using Microsoft.AspNetCore.SignalR;

namespace Yakify.Api;

public class StockHub : Hub
{
    public Task StockChanged()
    {
        return Clients.All.SendAsync("stockChanged");
    }
}