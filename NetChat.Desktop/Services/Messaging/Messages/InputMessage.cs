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
        public DateTime DateTime { get; set; }
        public User Sender { get; set; }
    }
}
