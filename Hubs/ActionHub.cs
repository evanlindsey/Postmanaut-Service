using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace PostmanautService.Hubs
{
    public class ActionHub : Hub
    {
        public static ConcurrentDictionary<string, int> clients = new ConcurrentDictionary<string, int>();

        public override Task OnConnectedAsync()
        {
            clients.TryAdd(Context.ConnectionId, 0);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception ex)
        {
            clients.TryRemove(Context.ConnectionId, out _);
            return base.OnDisconnectedAsync(ex);
        }
    }
}
