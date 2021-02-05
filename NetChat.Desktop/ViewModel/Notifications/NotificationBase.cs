using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat.Desktop.ViewModel.Notifications
{
    public abstract class NotificationBase
    {
        public NotificationBase(string title, string sender, string message)
        {
            Title = title;
            Sender = sender;
            Message = message;
        }

        public string Title { get; }
        public string Sender { get; }
        public string Message { get; }
    }
}
