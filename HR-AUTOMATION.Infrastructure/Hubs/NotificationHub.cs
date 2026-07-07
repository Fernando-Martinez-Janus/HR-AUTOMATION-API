using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR_AUTOMATION.Infrastructure.Hubs
{
    public class NotificationHub: Hub
    {
        public async Task ClientSendMessage( string user, string message)
        {
            await Clients.All.SendAsync("BroadcastMessage", user, message);
        }
    }
}
