using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat.Desktop.ViewModel.Notifications
{
    public class ParticipantNotificationItem : NotificationItem
    {
        public bool IsOnline { get; }

#if DEBUG
        public ParticipantNotificationItem() : base()
        {
            
        }
#endif

        public ParticipantNotificationItem(string participantName, bool isOnline)
            : base(null, participantName, isOnline ? "В сети" : "Ушел из сети")
        {
            IsOnline = isOnline;
        }
    }
}
