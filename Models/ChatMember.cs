using System;
using System.Collections.Generic;

namespace Messenger
{
    public partial class ChatMember
    {
        public int IdChat { get; set; }
        public int IdClient { get; set; }
        public string? Role { get; set; }
    }
}
