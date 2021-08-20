using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace SignalChat.Hubs
{
    public class ChatHub : Hub
    {
        public async Task Send(string message, string userName, string chatId) => await Clients.All.SendAsync("BroadcastMessage", message, userName, chatId);

        public async Task Edit(string newText, int messageId) => await Clients.All.SendAsync("EditMessage", newText, messageId);

        public async Task Delete(int messageId) => await Clients.All.SendAsync("DeleteMessage", messageId);

        public async Task DeleteRoom() => await Clients.All.SendAsync("DeleteRoom");

        public async Task Leave(string userName, string chatId) => await Clients.All.SendAsync("BroadcastMessage", "left room", userName, chatId);

        //public async Task Kick(string userName, string chatId) => await Clients.All.SendAsync("BroadcastMessage", "has been kicked", userName, chatId);
    }
}
