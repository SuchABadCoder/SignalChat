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

        public ChatController(IHubContext<ChatHub> chat, ApplicationDbContext context)
        {
            _chat = chat;
        }

        public async Task<IActionResult> SendMessage(
            string messageText,
            int chatId,
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

            await _chat.Clients.Group(chatId.ToString()).SendAsync("Send", message);

            return Ok();
        }
    }
}
