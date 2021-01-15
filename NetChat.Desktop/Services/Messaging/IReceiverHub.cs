using System;
using NetChat.Desktop.ViewModel.Messenger;

namespace NetChat.Desktop.Services.Messaging
{
    public interface IReceiverHub
    {
        void SubscribeMessageReceived(object sender, Action<MessageObservable> callback);
        void UnsubscribeMessageReceived(object sender);

        void SubscribeUserStatusChanged(object sender, Action<ParticipantObservable> callback);
        void UnsubscribeUserStatusChanged(object sender);
    }
}
