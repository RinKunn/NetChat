using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat.FileMessaging.Services
{
    public class CurrentUserContext
    {
        private static string _currentUserName;
        public static string CurrentUserName
        {
            get
            {
                if(_currentUserName == null)
                {
                    _currentUserName =
#if DEBUG
                        GetCurrentSenderId();
#else
                        Environment.UserName.ToUpper();
#endif
                }
                return _currentUserName;
            }
        }

#if DEBUG
        private static string GetCurrentSenderId()
        {
            string senderId = Environment.UserName;
            string currentProcess = Process.GetCurrentProcess().ProcessName;
            int runnedProc = Process.GetProcessesByName(currentProcess).Length;
            if (runnedProc > 1)
                senderId += runnedProc.ToString();
            return senderId.ToUpper();
        }
#endif
    }
}
