using Microsoft.AspNetCore.SignalR;
using SignalChat.Data;
using SignalChat.Models;
using System;
using System.Threading.Tasks;
using SignalChat.Controllers;

namespace SignalChat.Hubs
{
    public class ChatHub : Hub
    {
        public async Task Send(string message, string userName)
        {
            await Clients.All.SendAsync("BroadcastMessage", message, userName);
        }

    }
}
