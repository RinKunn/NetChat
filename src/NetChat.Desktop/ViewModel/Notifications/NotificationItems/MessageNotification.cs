using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace NetChat.Desktop.ViewModel.Notifications.NotificationItems
{
    public class MessageNotification : NotificationBase
    {

#if DEBUG
        public MessageNotification() : base()
        {

        }
#endif
        public MessageNotification(string id, string sender, string message)
            : base(id, "Новое сообщение", sender, message)
        {

        }
    }
}
