using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat.Desktop.InnerMessages
{
    public sealed class GoToMessageIM : IInnerMessage
    {
        public string MessageId { get; }

        public GoToMessageIM(string messageId)
        {
            MessageId = messageId;
        }
    }
}
