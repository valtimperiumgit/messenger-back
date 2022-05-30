using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Messenger
{
    public partial class ChatMessege
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? IdChat { get; set; }
        public int? IdClient { get; set; }
        public DateTime? Datetime { get; set; }
        public string? Body { get; set; }
    }
}
