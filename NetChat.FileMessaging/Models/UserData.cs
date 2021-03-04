using System;

namespace NetChat.FileMessaging.Models
{
    public class UserData
    {
        public string Id { get; set; }
        public bool IsOnline { get; set; }
        public DateTime LastChanged { get; set; }
    }
}
