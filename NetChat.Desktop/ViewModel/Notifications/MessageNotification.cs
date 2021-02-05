using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat.Desktop.ViewModel.Notifications
{
    public class MessageNotification : NotificationBase
    {
        public MessageNotification(string sender, string message) : base("Новое сообщение", sender, message)
        {

        }
    }
}
