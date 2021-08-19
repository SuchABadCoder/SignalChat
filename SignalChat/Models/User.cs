using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace SignalChat.Models
{
    public class User : IdentityUser
    {
        public static object Identity { get; internal set; }
        public ICollection<ChatUser> Chats { get; set; }
    }
}
