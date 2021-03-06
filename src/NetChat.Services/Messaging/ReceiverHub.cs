﻿//using System;
//using System.Collections.Concurrent;
//using System.Linq;
//using System.Threading.Tasks;
//using NetChat.FileMessaging.Services;
//using NetChat.FileMessaging.Services.Users;
//using NetChat.Services.Messaging.Messages;
//using NetChat.Services.Messaging.Users;
//using NLog;

//namespace NetChat.Desktop.Services.Messaging
//{
//    public sealed class ReceiverHub : IReceiverHub
//    {
//        private readonly ConcurrentDictionary<string, WeakReference<Action<Message>>> _mcallbacks
//            = new ConcurrentDictionary<string, WeakReference<Action<Message>>>();

//        private readonly ConcurrentDictionary<string, WeakReference<Action<User>>> _ucallbacks
//            = new ConcurrentDictionary<string, WeakReference<Action<User>>>();

//        private readonly object _locker = new object();
//        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
//        private readonly INetChatHub _hub;
//        private readonly IFileUserService _userService;

//        public ReceiverHub(INetChatHub hub, IFileUserService userService)
//        {
//            _hub = hub;
//            _userService = userService;++
//            _hub.OnMessageReceived += Hub_OnMessageReceived;
//            _hub.OnUserStatusChanged += Hub_OnUserStatusChanged;
//            _logger.Debug("ReceiverHub inited");
//        }

//        private Task Hub_OnUserStatusChanged(OnUserStatusChangedArgs args)
//        {
//            var u = args;
//            _logger.Debug("OnUserStatusChanged(l={0}): UserId='{1}', Online={2}", 
//                _ucallbacks.Skip(0).Count(), u.UserId, u.IsOnline);
//            var user = new ParticipantObservable(u.UserId, u.IsOnline, u.DateTime);

//            foreach (var act in _ucallbacks)
//            {
//                if (act.Value.TryGetTarget(out var action))
//                    action.Invoke(user);
//            }
//            return Task.CompletedTask;
//        }

//        private async Task Hub_OnMessageReceived(OnMessageReceivedArgs args)
//        {
//            var m = args.Message;
//            var user = await _userLoader.GetUserById(m.SenderId);
//            if(user == null)
//            {
//                user = new ParticipantObservable(m.SenderId, true, m.DateTime);
//                _logger.Warn("Created new default 'ParticipantObservable' for UsedId='{0}'", m.SenderId);
//            }
//            bool isMe = _userLoader.IsMe(m.SenderId);
//            _logger.Debug("OnMessageReceived(l={0}): id='{1}', UserId='{2}', Text={3}, Date={4:yyyy-MM-dd HH:mm:ss}",
//                _mcallbacks.Skip(0).Count(), m.Id, m.SenderId, m.Text, m.DateTime);
//            var message = new TextMessageObservable(m.Text, m.Id, m.DateTime, user, isMe);

//            foreach (var act in _mcallbacks)
//            {
//                if (act.Value.TryGetTarget(out var action))
//                    action.Invoke(message);
//            }
//        }


//        public void SubscribeMessageReceived(object sender, Action<MessageObservable> callback)
//        {
//            if (_mcallbacks.TryAdd(SenderObjToString(sender), new WeakReference<Action<MessageObservable>>(callback)))
//            {
//                _logger.Debug("'{0}' subscribed to message receiving", sender.GetType().Name);
//            }
//            else
//            {
//                _logger.Warn("Cannot add subscriber '{0}' to message receiving", sender.GetType().Name);
//            }
//        }

//        public void SubscribeUserStatusChanged(object sender, Action<ParticipantObservable> callback)
//        {
//            if(_ucallbacks.TryAdd(SenderObjToString(sender), new WeakReference<Action<ParticipantObservable>>(callback)))
//            {
//                _logger.Debug("'{0}' subscribed to user status receiving", sender.GetType().Name);
//            }
//            else
//            {
//                _logger.Warn("Cannot add subscriber '{0}' to user status receiving", sender.GetType().Name);
//            }
//        }

//        public void UnsubscribeMessageReceived(object sender)
//        {
//            string senderKey = SenderObjToString(sender);
//            if (!_mcallbacks.ContainsKey(senderKey))
//                return;
//            if(_mcallbacks.TryRemove(senderKey, out var act))
//            {
//                _logger.Debug("'{0}' unsubscribed from message receiving", sender.GetType().Name);
//            }
//            else
//            {
//                _logger.Warn("Cannot remove subscriber '{0}' from message receiving", sender.GetType().Name);
//            }
//        }

//        public void UnsubscribeUserStatusChanged(object sender)
//        {
//            string senderKey = SenderObjToString(sender);
//            if (!_ucallbacks.ContainsKey(senderKey))
//                return;
//            if (_ucallbacks.TryRemove(senderKey, out var act))
//            {
//                _logger.Debug("'{0}' unsubscribed from message receiving", sender.GetType().Name);
//            }
//            else
//            {
//                _logger.Warn("Cannot remove subscriber '{0}' from message receiving", sender.GetType().Name);
//            }
//        }

//        public void Connect() => _hub?.Connect();

//        public void Disconnect() => _hub?.Disconnect();

//        public bool IsConnected => _hub.IsConnected;

//        private string SenderObjToString(object sender)
//        {
//            return sender.GetType().ToString() + sender.GetHashCode();
//        }

//        public void Dispose()
//        {
//            if (_hub.IsConnected)
//                _hub.Disconnect();
//            lock (_locker)
//            {
//                _mcallbacks.Clear();
//                _ucallbacks.Clear();
//            }
//            _hub.Dispose();
//        }
//    }
//}
