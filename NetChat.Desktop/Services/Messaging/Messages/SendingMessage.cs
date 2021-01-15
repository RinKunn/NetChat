using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat.Desktop.Services.Messaging.Messages
{
    public class SendingMessage
    {
        public SendingMessage(string userId)
        {
            UserId = userId;
        }

        public string UserId { get; }
    }
}
