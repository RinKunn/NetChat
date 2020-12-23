using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetChat.Desktop.Services.Messaging.Users;

namespace NetChat.Desktop.Services.Messaging.Messages
{
    public abstract class InputMessage
    {
        public DateTime DateTime { get; private set; }
        public string UserId { get; private set; }

        public InputMessage(string userId)
        {
            DateTime = DateTime.Now;
            UserId = userId;
        }
    }
}
