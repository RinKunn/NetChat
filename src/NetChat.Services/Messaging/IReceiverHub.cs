using System;
using NetChat.Services.Messaging.Messages;
using NetChat.Services.Messaging.Users;

namespace NetChat.Desktop.Services.Messaging
{
    public interface IReceiverHub : IDisposable
    {
        void SubscribeMessageReceived(object sender, Action<Message> callback);
        void UnsubscribeMessageReceived(object sender);

        void SubscribeUserStatusChanged(object sender, Action<User> callback);
        void UnsubscribeUserStatusChanged(object sender);

        void Connect();
        void Disconnect();

        bool IsConnected { get; }
    }
}
