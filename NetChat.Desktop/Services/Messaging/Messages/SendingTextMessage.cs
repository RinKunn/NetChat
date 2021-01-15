using System;

namespace NetChat.Desktop.Services.Messaging.Messages
{
    public class SendingTextMessage : SendingMessage
    {
        public string Text { get; }

        public SendingTextMessage(string userId, string text) : base(userId)
        {
            Text = text;
        }
    }
}
