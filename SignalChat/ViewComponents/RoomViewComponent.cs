using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SignalChat.Data;
using System.Linq;
using System.Security.Claims;

namespace SignalChat.ViewComponents
{
    public class RoomViewComponent : ViewComponent
    {
        private ApplicationDbContext _context;

        public RoomViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var chats = _context.ChatUsers
                    .Include(x => x.Chat)
                    .Where(x => x.UserId == userId)
                    .Select(x => x.Chat)
                    .ToList();
            return View(chats);
        }
    }
}
