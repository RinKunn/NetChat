using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat.Desktop.Services.Messaging.Messages
{
    public abstract class InputMessageContent
    {

    }

    public class InputMessageTextContent : InputMessageContent
    {
        public string Text { get; }

        public InputMessageTextContent(string text)
        {
            Text = text;
        }
    }
}
