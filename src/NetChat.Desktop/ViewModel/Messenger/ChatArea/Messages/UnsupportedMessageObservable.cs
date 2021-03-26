using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat.Desktop.ViewModel.Messenger.ChatArea.Messages
{
    public sealed class UnsupportedMessageObservable : MessageObservable
    {
        public UnsupportedMessageObservable(string messageId, DateTime dateTime,
            string authorName, bool isOutgoing, ReplyObservable reply = null)
            : base(messageId, dateTime, authorName, isOutgoing, reply)
        { }
    }
}
