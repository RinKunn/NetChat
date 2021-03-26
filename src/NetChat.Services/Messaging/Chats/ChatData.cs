using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat.Services.Messaging.Chats
{
    public class ChatData
    {
        public ChatData(string lastReadInboxMessageId)
        {
            LastReadInboxMessageId = lastReadInboxMessageId;
        }

        public string LastReadInboxMessageId { get; }
    }
}
