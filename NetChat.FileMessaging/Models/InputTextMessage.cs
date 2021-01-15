using System;

namespace NetChat.FileMessaging.Models
{
    public class InputTextMessage
    {
        public DateTime DateTime { get; }
        public string UserId { get; }
        public string Text { get; }

        public InputTextMessage(string userId, string text)
        {
            DateTime = DateTime.Now;
            UserId = userId;
            Text = text;
        }
    }
}
