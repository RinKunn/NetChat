using System;

namespace NetChat.Services.Messaging.Messages
{
    public class MessageData
    {
        public MessageData(string id,
            string senderId,
            DateTime date,
            bool isOutgoing,
            MessageContent content,
            string replyToMessageId)
        {
            Id = id;
            SenderId = senderId;
            Date = date;
            IsOutgoing = isOutgoing;
            Content = content;
            ReplyToMessageId = replyToMessageId;
        }

        public string Id { get; }
        public string SenderId { get; }
        public DateTime Date { get; }
        public bool IsOutgoing { get; }
        public MessageContent Content { get; }
        public string ReplyToMessageId { get; }
    }

    // TODO: add editing, editing date

    public abstract class MessageContent
    {

    }

    public class MessageTextContent : MessageContent
    {
        public string Text { get; }
        public MessageTextContent(string text)
        {
            Text = text;
        }
    }
}
