using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat.Desktop.ViewModel.Notifications.NotificationItems
{
    public class ParticipantNotification : NotificationBase
    {
        public bool IsOnline { get; }

#if DEBUG
        public ParticipantNotification() : base(null, null, "User name", "В сети")
        {

        }
#endif

        public ParticipantNotification(string participantName, bool isOnline)
            : base(null, null, participantName, isOnline ? "В сети" : "Ушел из сети")
        {
            IsOnline = isOnline;
        }
    }
}
