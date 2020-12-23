using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetChat.Desktop.Services.Messaging.Users;

namespace NetChat.Desktop.Services.Messaging.Messages
{
    public class InputMessageText : InputMessage
    {
        public string Text { get; private set; }

        public InputMessageText(string text, string userId) : base(userId)
        {
            Text = text;
        }
    }
}
