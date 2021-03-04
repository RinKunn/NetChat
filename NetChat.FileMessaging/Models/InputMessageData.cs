using System;

namespace NetChat.FileMessaging.Models
{
    public class InputMessageData
    {
        public DateTime DateTime { get; }
        public string SenderId { get; }
        public string Text { get; }
        public string ReplyToMessageId { get; }

        public InputMessageData(string senderId, string text, string replyToMessageId)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentNullException("Sending message must have not empty 'Text'");
            if (string.IsNullOrEmpty(senderId))
                throw new ArgumentNullException("Sending message must have not empty 'SenderId'");
            DateTime = DateTime.Now;
            SenderId = senderId;
            Text = text;
            ReplyToMessageId = replyToMessageId;
        }
    }
}
