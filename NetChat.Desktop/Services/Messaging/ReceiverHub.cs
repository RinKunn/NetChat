using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using NetChat.Desktop.Services.Messaging.Users;
using NetChat.Desktop.ViewModel.Messenger;
using NetChat.FileMessaging.Services;
using NLog;

namespace NetChat.Desktop.Services.Messaging
{
    public class ReceiverHub : IReceiverHub
    {
        private readonly ConcurrentDictionary<object, Action<MessageObservable>> _mcallbacks
            = new ConcurrentDictionary<object, Action<MessageObservable>>();
        private readonly ConcurrentDictionary<object, Action<ParticipantObservable>> _ucallbacks
            = new ConcurrentDictionary<object, Action<ParticipantObservable>>();

        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly INetChatHub _hub;
        private readonly IUserLoader _userLoader;

        public ReceiverHub(INetChatHub hub, IUserLoader userLoader)
        {
            _logger.Debug("ReceiverHub initing...");
            _hub = hub;
            _userLoader = userLoader;
            _hub.OnMessageReceived += _hub_OnMessageReceived;
            _hub.OnUserStatusChanged += _hub_OnUserStatusChanged;
            _logger.Debug("ReceiverHub inited");
        }

        private Task _hub_OnUserStatusChanged(OnUserStatusChangedArgs args)
        {
            var u = args;
            _logger.Debug("UserStatusChanged: {0} - {1}", u.UserId, u.IsOnline);
            var user = new ParticipantObservable(u.UserId, u.IsOnline, u.DateTime);
            foreach (var key in _ucallbacks.Keys)
            {
                _ucallbacks[key].Invoke(user);
            }
            return Task.CompletedTask;
        }

        private async Task _hub_OnMessageReceived(OnMessageReceivedArgs args)
        {
            var m = args.Message;
            _logger.Debug("OnMessageReceived: {0} - {1}", m.SenderId, m.Text);
            var user = await _userLoader.GetUserById(m.SenderId);
            bool isMe = _userLoader.IsMe(m.SenderId);
            var message = new TextMessageObservable(m.Text, m.Id, m.DateTime, user, isMe);
            
            foreach (var key in _mcallbacks.Keys)
            {
                _mcallbacks[key].Invoke(message);
            }
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

        public void Connect() => _hub?.Connect();

        public void Disconnect() => _hub?.Disconnect();
    }
}
