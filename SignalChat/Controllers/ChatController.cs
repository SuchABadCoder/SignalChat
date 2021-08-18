using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalChat.Data;
using SignalChat.Hubs;
using SignalChat.Models;
using System.Threading.Tasks;

namespace SignalChat.Controllers
{
    [Authorize]
    [Route("controller")]
    public class ChatController : Controller
    {
        private IHubContext<ChatHub> _chat;

        public ChatController(IHubContext<ChatHub> chat)
        {
            _chat = chat;
        }

        [HttpPost("[action]/{connectionId}/{roomName}")]
        public async Task<IActionResult> JoinRoom(string connectionId, string roomName)
        {
            await _chat.Groups.AddToGroupAsync(connectionId, roomName);

            return Ok();
        }

        [HttpPost("[action]/{connectionId}/{roomName}")]
        public async Task<IActionResult> LeaveRoom(string connectionId, string roomName)
        {
            await _chat.Groups.RemoveFromGroupAsync(connectionId, roomName);

            return Ok();
        }

        public async Task<IActionResult> SendMessage(
            string messageText,
            int chatId, 
            string roomName,
            [FromServices] ApplicationDbContext context)
        {
            var message = new Message
            {
                ChatId = chatId,
                Text = messageText,
                UserName = User.Identity.Name,
                DateTime = System.DateTime.Now
            };

            context.Messages.Add(message);
            await context.SaveChangesAsync();

            await _chat.Clients.Group(roomName).SendAsync("RecieveMessage", message);

            return Ok();
        }
    }
}
