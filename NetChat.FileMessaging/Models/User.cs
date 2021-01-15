using System;

namespace NetChat.FileMessaging.Models
{
    public class User
    {
        public string Id { get; set; }
        public bool IsOnline { get; set; }
        public DateTime LastChanged { get; set; }
    }
}
