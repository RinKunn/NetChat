using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat.Desktop.ViewModel.Notifications
{
    public class NotificationConfiguration
    {
        public bool EnableParticipantNotifications { get; set; } = false;
        public bool EnableMessageNotifications { get; set; } = true;
        public TimeSpan HideTimeout { get; set; } = TimeSpan.FromSeconds(5);
        public int ShowingMaxCount { get; set; } = 3;
    }
}
