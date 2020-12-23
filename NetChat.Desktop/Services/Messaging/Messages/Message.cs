using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetChat.Desktop.Services.Messaging.Users;

namespace NetChat.Desktop.Services.Messaging.Messages
{
    public abstract class Message
    {
        public string Id { get; set; }
        public DateTime DateTime { get; set; }
        public User Sender { get; set; }
        public bool IsOriginNative { get; set; }
    }
}
