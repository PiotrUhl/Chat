using System;
using System.Collections.Generic;

#nullable disable

namespace Database.Model
{
    public partial class Client
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public byte[] Password { get; set; }
        public string DisplayName { get; set; }
    }
}
