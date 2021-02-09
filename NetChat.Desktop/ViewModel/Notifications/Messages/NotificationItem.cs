using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace NetChat.Desktop.ViewModel.Notifications
{
    public class NotificationItem : ObservableObject
    {
#if DEBUG
        public NotificationItem()
        {
            if (ViewModelBase.IsInDesignModeStatic)
            {
                Title = "Title";
                Sender = "Sender A.A.";
                Message = "Message from sender long very long message aga or not aga";
            }
            else throw new NotImplementedException();
        }
#endif

        public NotificationItem(string id, string title, string sender, string message)
        {
            MessageId = id;
            Title = title;
            Sender = sender;
            Message = message;
        }

        public string MessageId { get; }
        public string Title { get; }
        public string Sender { get; }
        public string Message { get; }

        private bool _isClosing;
        public bool IsClosing
        {
            get => _isClosing;
            set => Set(ref _isClosing, value);
        }
    }
}
