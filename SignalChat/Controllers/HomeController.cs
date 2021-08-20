using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SignalChat.Data;
using SignalChat.Models;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
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

        public async Task<IActionResult> Index()
        {
            var chats = await _context.Chats
                .Include(x => x.Users)
                .Where(x => !x.Users.Any(y => y.UserId == User.FindFirst(ClaimTypes.NameIdentifier).Value) 
                && !x.isClosed)
                .ToListAsync();

            return View(chats);
        }

        //public IActionResult Find()
        //{
        //    var users = _context.Users
        //        .Where(x => x.Id != User.FindFirst(ClaimTypes.NameIdentifier).Value)
        //        .ToList();

        //    return View(users);
        //}

        //public IActionResult Private()
        //{
        //    var chats = _context.Chats
        //        .Include(x => x.Users)
        //            .ThenInclude(x => x.User)
        //        .Where(x => x.Type == ChatType.Private
        //            && x.Users
        //            .Any(y => y.UserId == User.FindFirst(ClaimTypes.NameIdentifier).Value))
        //        .ToList();
        //    ViewBag.UserId = ClaimTypes.NameIdentifier;
        //    return View(chats);
        //}

        //public async Task<IActionResult> CreatePrivateRoom(string userId)
        //{
        //    var chat = new Chat
        //    {
        //        Type = ChatType.Private
        //    };

        //    chat.Users.Add(new ChatUser
        //    {
        //        UserId = userId
        //    });

        //    chat.Users.Add(new ChatUser
        //    {
        //        UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value
        //    });

        //    _context.Chats.Add(chat);
        //    await _context.SaveChangesAsync();

        //    return RedirectToAction("Chat", new { id = chat.Id });
        //}

        [HttpGet("{id}")]
        public IActionResult Chat(int id)
        {
            var UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var chat = _context.Chats
                .Include(x => x.Messages)
                .FirstOrDefault(x => x.Id == id);
            var UserRole = _context.ChatUsers.Where(x => x.ChatId == id && x.UserId == UserId).FirstOrDefault().Role;
            ViewBag.UserRole = UserRole;
            ViewBag.UserName = User.Identity.Name;
            return View(chat);
        }

        //[HttpPost]
        //public async Task<IActionResult> CreateMessage(int chatId, string messageText)
        //{
        //    var message = new Message
        //    {
        //        ChatId = chatId,
        //        Text = messageText,
        //        UserName = User.Identity.Name,
        //        DateTime = System.DateTime.Now
        //    };

        //    _context.Messages.Add(message);
        //    await _context.SaveChangesAsync();

        //    return RedirectToAction("Chat", new { id = chatId });
        //}

        [HttpPost]
        public async Task<IActionResult> CreateRoom(string name)
        {
            var chat = new Chat
            {
                Name = name,
                Type = ChatType.Room,
                isClosed = false
            };

            chat.Users.Add(new ChatUser
            {
                UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value, //search how it works
                Role = UserRole.Admin
            });

            _context.Chats.Add(chat);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public async Task<IActionResult> JoinRoom(int id)
        {
            var chatUser = new ChatUser
            {
                ChatId = id,
                UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                Role = UserRole.Member
            };

            _context.ChatUsers.Add(chatUser);
            await _context.SaveChangesAsync();

            return RedirectToAction("Chat", "Home", new { id = id });
        }

        public async Task<IActionResult> EditMessage(int messageId, string newText)
        {
            var mes = _context.Messages.Where(x => x.Id == messageId).FirstOrDefault();
            mes.Text = newText;
            await _context.SaveChangesAsync();
            return Ok();

        }

        public async Task<IActionResult> DeleteMessage(int messageId)
        {

            _context.Messages.Remove(_context.Messages.Where(x => x.Id == messageId).FirstOrDefault());
            await _context.SaveChangesAsync();

            return Ok();
        }
        //public async Task<JsonResult> GetMessageId(string UserName, string Text)
        //{

        //    var Id = await _context.Messages.Where(x => x.Text == Text && x.UserName == UserName).ToListAsync();
        //    return Json(Id);
        //}
        public async Task<JsonResult> MakePrivate(int chatId)
        {
            var room = _context.Chats.Where(x => x.Id == chatId).FirstOrDefault();
            room.isClosed = true;
            await _context.SaveChangesAsync();

            return Json(true);
        }

        public async Task<JsonResult> MakePublic(int chatId)
        {
            var room = _context.Chats.Where(x => x.Id == chatId).FirstOrDefault();
            room.isClosed = false;
            await _context.SaveChangesAsync();

            return Json(true);
        }

        [HttpPost]
        public async Task<JsonResult> DeleteRoom(int chatId)
        {

            _context.Chats.Remove(_context.Chats.Where(x => x.Id == chatId).FirstOrDefault());
            await _context.SaveChangesAsync();

            return Json(true);
        }

        public async Task<JsonResult> LeaveRoom(int chatId)
        {
            var UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            _context.ChatUsers.Remove(_context.ChatUsers.Where(x => x.ChatId == chatId && x.UserId == UserId).FirstOrDefault());
            //_context.Chats.Where(x => x.Id == chatId).FirstOrDefault()
            //    .Users.Remove(_context.ChatUsers.Where(y => y.UserId == UserId).FirstOrDefault());
            await _context.SaveChangesAsync();

            return Json(true);
        }
    }
}
