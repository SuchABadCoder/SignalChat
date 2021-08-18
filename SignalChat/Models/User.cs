using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace SignalChat.Models
{
    public class User : IdentityUser
    {
        public ICollection<ChatUser> Chats { get; set; }
    }
}
