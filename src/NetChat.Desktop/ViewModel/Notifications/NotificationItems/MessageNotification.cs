using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat.Desktop.ViewModel.Notifications
{
    public class MessageNotification : NotificationBase
    {
        public MessageNotification(string id, string sender, string message) 
            : base(id, "Новое сообщение", sender, message)
        {

        }
    }
}
