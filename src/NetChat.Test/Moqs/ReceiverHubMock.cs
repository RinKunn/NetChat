using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetChat.Desktop.Services.Messaging;
using NetChat.Desktop.ViewModel.Messenger.ChatArea.Messages;

namespace NetChat.Test.Moqs
{
    public sealed class ReceiverHubMock : IReceiverHub
    {
        public bool _isConnected;
        public bool IsConnected => _isConnected;

        private readonly Dictionary<object, Action<MessageObservable>> mess = new Dictionary<object, Action<MessageObservable>>(); 
        private readonly Dictionary<object, Action<ParticipantObservable>> partic = new Dictionary<object, Action<ParticipantObservable>>(); 


        public void Connect()
        {
            _isConnected = true;
        }

        public void Disconnect()
        {
            _isConnected = false;
            mess.Clear();
            partic.Clear();
        }

        public void Dispose()
        {
            
        }

        public void SubscribeMessageReceived(object sender, Action<MessageObservable> callback)
        {
            mess.Add(sender, callback);
        }

        public void SubscribeUserStatusChanged(object sender, Action<ParticipantObservable> callback)
        {
            partic.Add(sender, callback);
        }

        public void UnsubscribeMessageReceived(object sender)
        {
            mess.Remove(sender);
        }

        public void UnsubscribeUserStatusChanged(object sender)
        {
            partic.Remove(sender);
        }



        public void RaiseMessageReceived(MessageObservable message)
        {
            if (!_isConnected) return;
            foreach(var callback in mess.Values)
            {
                callback?.Invoke(message);
            }
        }

        public void RaiseParticipantReceived(ParticipantObservable participant)
        {
            if (!_isConnected) return;
            foreach (var callback in partic.Values)
            {
                callback?.Invoke(participant);
            }
        }
    }
}
