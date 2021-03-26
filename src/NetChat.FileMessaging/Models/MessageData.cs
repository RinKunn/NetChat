using System;

namespace NetChat.FileMessaging.Models
{
    public class MessageData
    {
        public MessageData(string id, DateTime dateTime, string senderId, 
            string text, bool isOutgoing, string replyToMessageId)
        {
            Id = id;
            DateTime = dateTime;
            SenderId = senderId;
            Text = text;
            ReplyToMessageId = replyToMessageId;
            IsOutgoing = isOutgoing;
        }

        public string Id { get; }
        public DateTime DateTime { get; }
        public string SenderId { get; }
        public string Text { get; }
        public string ReplyToMessageId { get; }
        public bool IsOutgoing { get; }
    }
}
