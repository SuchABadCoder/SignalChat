using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalChat.Data;
using SignalChat.Hubs;
using System.Linq;
using SignalChat.Models;
using System.Threading.Tasks;

namespace SignalChat.Controllers
{
    [Authorize]
    [Route("controller")]
    public class ChatController : Controller
    {
        private IHubContext<ChatHub> _chat;
        //private ApplicationDbContext _context;
        //private object locker = new object();
        public ChatController(IHubContext<ChatHub> chat, ApplicationDbContext context)
        {
            _chat = chat;
           // _context = context;
        }

        //[HttpPost("[action]/{connectionId}/{roomId}")]
        //public async Task<IActionResult> JoinRoom(string connectionId, string roomId)
        //{
        //    await _chat.Groups.AddToGroupAsync(connectionId, roomId);

        //    return Ok();
        //}

        //[HttpPost("[action]/{connectionId}/{roomId}")]
        //public async Task<IActionResult> LeaveRoom(string connectionId, string roomId)
        //{
        //    await _chat.Groups.RemoveFromGroupAsync(connectionId, roomId);

        //    return Ok();
        //}

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
