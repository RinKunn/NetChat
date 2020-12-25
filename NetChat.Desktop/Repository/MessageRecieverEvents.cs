using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat.Desktop.Repository
{
    public delegate void OnMessageReceivedHandler(MessageData message);
    public delegate void OnUserLoggedInHandler(OnUserStatusChangedArgs args);
    public delegate void OnUserLoggedOutHandler(OnUserStatusChangedArgs args);

    public class OnUserStatusChangedArgs
    {
        public string UserId { get; private set; }
        public DateTime DateTime { get; private set; }

        public OnUserStatusChangedArgs(string userId, DateTime datetime)
        {
            UserId = userId;
            DateTime = datetime;
        }
    }
}
