using Microsoft.AspNetCore.Mvc;
using SignalChat.Data;
using System.Linq;

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
            var chats = _context.Chats.ToList();
            return View(chats);
        }
    }
}
