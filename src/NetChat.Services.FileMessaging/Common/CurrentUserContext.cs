using System;
using System.Diagnostics;

namespace NetChat.Services.FileMessaging.Common
{
    public class CurrentUserContext
    {
        private static string _currentUserName;
        public static string CurrentUserName
        {
            get
            {
                if (_currentUserName == null)
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


    }
}
