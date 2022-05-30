using System;
using System.Collections.Generic;

namespace Messenger
{
    public partial class ClientAvatar
    {
        public int Id { get; set; }
        public int? IdClient { get; set; }
        public byte[]? Photo { get; set; }
    }
}
