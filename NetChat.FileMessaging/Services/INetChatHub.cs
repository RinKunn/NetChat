using System;

namespace NetChat.FileMessaging.Services
{
    public interface INetChatHub : IDisposable
    {
        event OnMessageReceivedHandler OnMessageReceived;
        event OnUserStatusChangedHandler OnUserStatusChanged;

        void Connect();
        void Disconnect();
        bool IsConnected { get; }
    }
}
