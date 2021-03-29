using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetChat.Services.Messaging.Messages;

namespace NetChat.Desktop.InnerMessages
{
    public sealed class ReplyToMessageIM : IInnerMessage
    {
        public ReplyToMessageIM(string messageId, string authorName, string text)
        {
            MessageId = messageId;
            AuthorName = authorName;
            Text = text;
        }

        public string MessageId { get; }
        public string AuthorName { get; }
        public string Text { get; }
    }
}
