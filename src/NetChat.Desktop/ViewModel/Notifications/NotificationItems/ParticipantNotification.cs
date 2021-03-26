using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat.Desktop.ViewModel.Notifications
{
    public class ParticipantNotification : NotificationBase
    {
        public bool IsOnline { get; }

#if DEBUG
        public ParticipantNotification() : base()
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
