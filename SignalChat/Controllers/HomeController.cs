using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SignalChat.Data;
using SignalChat.Models;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SignalChat.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("{id}")]
        public IActionResult Chat(int id)
        {
            var chat = _context.Chats
                .Include(x => x.Messages)
                .FirstOrDefault(x => x.Id == id);
            return View(chat);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(int chatId, string messageText)
        {
            var message = new Message
            {
                ChatId = chatId,
                Text = messageText,
                UserName = User.Identity.Name,
                DateTime = System.DateTime.Now
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return RedirectToAction("Chat", new { id = chatId });
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoom(string name)
        {
            _context.Chats.Add(new Chat{
                Name = name,
                Type = ChatType.Room
            });

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
