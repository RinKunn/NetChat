using System;

namespace NetChat.FileMessaging.Models
{
    public class TextMessage
    {
        public string Id { get; set; }
        public DateTime DateTime { get; set; }
        public string SenderId { get; set; }
        public string Text { get; set; }
    }
}
