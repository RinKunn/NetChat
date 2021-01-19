using System;

namespace NetChat.FileMessaging.Services
{
    public interface INetChatHub : IDisposable
    {
        event OnMessageReceivedHandlerAsync OnMessageReceived;
        event OnUserStatusChangedHandlerAsync OnUserStatusChanged;

        void Connect();
        void Disconnect();
        bool IsConnected { get; }
    }
}
