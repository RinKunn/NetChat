using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat.Desktop.ViewModel.Notifications
{
    public class ParticipantNotification : NotificationBase
    {
        public ParticipantNotification(string participantName, bool isOnline)
            : base(null, participantName, isOnline ? "В сети" : "Ушел из сети")
        {

        }
    }
}
