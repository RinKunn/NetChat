using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat.Desktop.ViewModel.InnerMessages
{
    public class NotificationEnablingInnerMessage : NoticeInnerMessage
    {
        public bool Enable { get; }

        public NotificationEnablingInnerMessage(bool enable)
        {
            Enable = enable;
        }
    }
}
