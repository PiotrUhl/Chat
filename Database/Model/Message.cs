using System;
using System.Collections.Generic;

#nullable disable

namespace Chat.Database.Model
{
    public partial class Message
    {
        public long Id { get; set; }
        public int SenderId { get; set; }
        public int RecipentId { get; set; }
        public string Text { get; set; }
        public DateTime SendingTime { get; set; }
        public bool Delivered { get; set; }
        public bool Displayed { get; set; }
    }
}
