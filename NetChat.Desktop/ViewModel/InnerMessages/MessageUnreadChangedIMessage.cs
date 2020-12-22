using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat.Desktop.ViewModel.InnerMessages
{
    public class MessageUnreadChangedIMessage
    {
        public int Count;

        public MessageUnreadChangedIMessage(int count)
        {
            Count = count;
        }
    }
}
