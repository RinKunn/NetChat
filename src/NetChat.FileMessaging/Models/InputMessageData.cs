using System;
using System.Diagnostics;
using NetChat.FileMessaging.Services;

namespace NetChat.FileMessaging.Models
{
    public class InputMessageData
    {
        public DateTime DateTime { get; }
        public string SenderId { get; }
        public string Text { get; }
        public string ReplyToMessageId { get; }

        public InputMessageData(string text, string replyToMessageId)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentNullException("Sending message must have not empty 'Text'");

            SenderId = CurrentUserContext.CurrentUserName;
            DateTime = DateTime.Now;
            Text = text;
            ReplyToMessageId = replyToMessageId;
        }
    }
}
