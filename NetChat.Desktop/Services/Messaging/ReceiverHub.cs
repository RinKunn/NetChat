using System;
using System.Collections.Concurrent;
using NetChat.Desktop.Services.Messaging.Messages;
using NetChat.Desktop.Services.Messaging.Users;
using NetChat.Desktop.ViewModel.Messenger;
using NetChat.FileMessaging.Services;
using NetChat.FileMessaging.Models;

namespace NetChat.Desktop.Services.Messaging
{
    public class ReceiverHub : IReceiverHub
    {
        private readonly ConcurrentDictionary<object, Action<MessageObservable>> _mcallbacks
            = new ConcurrentDictionary<object, Action<MessageObservable>>();
        private readonly ConcurrentDictionary<object, Action<ParticipantObservable>> _ucallbacks
            = new ConcurrentDictionary<object, Action<ParticipantObservable>>();

        private readonly ConcurrentQueue<TextMessage> _messagesQueue = new ConcurrentQueue<TextMessage>();
        private readonly ConcurrentQueue<OnUserStatusChangedArgs> _usersQueue = new ConcurrentQueue<OnUserStatusChangedArgs>();


        private readonly INetChatHub _hub;
        private readonly IUserLoader _userLoader;

        public ReceiverHub(INetChatHub hub, IUserLoader userLoader)
        {
            _hub = hub;
            _userLoader = userLoader;
            _hub.OnMessageReceived += _hub_OnMessageReceived;
            _hub.OnUserStatusChanged += _hub_OnUserStatusChanged;
        }

        private void _hub_OnUserStatusChanged(OnUserStatusChangedArgs args)
        {
            
        }

        private void _hub_OnMessageReceived(TextMessage message)
        {
            //TODO Handle 
            var user = await _userLoader.GetUserById(message.SenderId);
            bool isMe = _userLoader.IsMe(user.UserId);
            return new TextMessageObservable(m.Text, m.Id, m.DateTime, user, isMe);
        }


        public void SubscribeMessageReceived(object sender, Action<MessageObservable> callback)
        {
            _mcallbacks.AddOrUpdate(sender, callback, (k, v) => callback);
        }

        public void SubscribeUserStatusChanged(object sender, Action<ParticipantObservable> callback)
        {
            _ucallbacks.AddOrUpdate(sender, callback, (k, v) => callback);
        }

        public void UnsubscribeMessageReceived(object sender)
        {
            if (!_mcallbacks.ContainsKey(sender)) return;
            _mcallbacks.TryRemove(sender, out var callback);
        }

        public void UnsubscribeUserStatusChanged(object sender)
        {
            if (!_ucallbacks.ContainsKey(sender)) return;
            _ucallbacks.TryRemove(sender, out var callback);
        }

        private void _messageReceiver_OnMessageReceived(MessageData messageData)
        {
            Message message = new TextMessage()
            {
                Id = messageData.Id,
                DateTime = messageData.DateTime,
                IsOriginNative = _userLoader.IsMe(messageData.UserId),
                Text = ((MessageTextData)messageData).Text,
                Sender = _userLoader.GetUserById(messageData.UserId).Result
            };

            foreach (var key in _mcallbacks.Keys)
            {
                _mcallbacks[key].Invoke(message);
            }
        }

        private void _messageReceiver_OnUserStatusChanged(OnUserStatusChangedArgs args)
        {
            User user = new User()
            {
                Id = args.UserId,
                IsOnline = args.IsOnline,
                LastChanged = args.DateTime
            };

            foreach (var key in _ucallbacks.Keys)
            {
                _ucallbacks[key].Invoke(user);
            }
        }
    }
}
