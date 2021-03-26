using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat.Desktop.InnerMessages
{
    public sealed class NotificationEnablingIM : IInnerMessage
    {
        public bool Enable { get; }

        public NotificationEnablingIM(bool enable)
        {
            Enable = enable;
        }
    }
}
