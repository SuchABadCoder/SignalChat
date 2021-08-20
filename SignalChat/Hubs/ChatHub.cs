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
        public async Task Send(string message, string userName, string chatId)
        {
            await Clients.All.SendAsync("BroadcastMessage", message, userName, chatId);
        }

        public async Task Edit(string newText, int messageId)
        {
            await Clients.All.SendAsync("EditMessage", newText, messageId);
        }

        public async Task Delete(int messageId)
        {
            await Clients.All.SendAsync("DeleteMessage", messageId);
        }

        public async Task DeleteRoom() => await Clients.All.SendAsync("DeleteRoom");
    }
}
