using System;
using System.Threading.Tasks;
using NetChat.FileMessaging.Models;


namespace NetChat.FileMessaging.Services
{
    public delegate Task OnMessageReceivedHandlerAsync(OnMessageReceivedArgs message);
    public delegate Task OnUserStatusChangedHandlerAsync(OnUserStatusChangedArgs args);

    public class OnMessageReceivedArgs
    {
        public MessageData Message { get; }

        public OnMessageReceivedArgs(MessageData message)
        {
            Message = message ?? throw new ArgumentNullException(nameof(message));
        }
    }

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
