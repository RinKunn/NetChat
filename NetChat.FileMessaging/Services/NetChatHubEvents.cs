using System;
using NetChat.FileMessaging.Models;


namespace NetChat.FileMessaging.Services
{
    public delegate void OnMessageReceivedHandler(TextMessage message);
    public delegate void OnUserStatusChangedHandler(OnUserStatusChangedArgs args);

    public class OnUserStatusChangedArgs
    {
        public string UserId { get; }
        public bool IsOnline { get; }
        public DateTime DateTime { get; }

        public OnUserStatusChangedArgs(string userId, bool isOnline, DateTime datetime)
        {
            UserId = userId;
            IsOnline = isOnline;
            DateTime = datetime;
        }
    }
}
