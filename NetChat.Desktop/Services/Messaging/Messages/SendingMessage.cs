using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat.Desktop.Services.Messaging.Messages
{
    public class SendingMessage
    {
        public SendingMessage(string senderId, InputMessageContent content, string replyToMessageId)
        {
            SenderId = senderId;
            Content = content;
        }

        public string SenderId { get; }
        public InputMessageContent Content { get; }
        public string ReplyToMessageId { get; }
    }
}
