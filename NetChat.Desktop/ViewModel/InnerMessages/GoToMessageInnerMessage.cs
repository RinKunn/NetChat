using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat.Desktop.ViewModel.InnerMessages
{
    public class GoToMessageInnerMessage : NoticeInnerMessage
    {
        public string MessageId { get; }

        public GoToMessageInnerMessage(string messageId)
        {
            MessageId = messageId;
        }
    }
}
