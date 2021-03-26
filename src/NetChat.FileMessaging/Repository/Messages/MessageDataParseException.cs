using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat.FileMessaging.Repository.Messages
{
    public class MessageDataParseException : Exception
    {
        public string Line { get; }

        public MessageDataParseException(string message, string line)
            : this(message, line, null) { }

        public MessageDataParseException(string message, string line, Exception innerException)
            : base(message, innerException)
        {
            Line = line;
        }
    }
}
