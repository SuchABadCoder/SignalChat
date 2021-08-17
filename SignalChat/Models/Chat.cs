﻿using System.Collections.Generic;

namespace SignalChat.Models
{
    public class Chat
    {
        public int Id { get; set; }

        public ChatType Type { get; set; }

        public ICollection<Message> Messages { get; set; }

        public ICollection<User> Users { get; set; }
    }
}