//using System;
//using System.Collections.Concurrent;
//using System.Threading.Tasks;
//using NetChat.FileMessaging.Services.Messages;
//using NetChat.FileMessaging.Services.Users;

//namespace NetChat.Services.Messaging.Messages
//{
//    public class MessageLoaderScope : IDisposable
//    {
//        private readonly IFileMessageService _messageService;
//        private readonly IFileUserService _userService;

//        private readonly ConcurrentDictionary<string, UserData> _users
//            = new ConcurrentDictionary<string, UserData>();

//        private readonly ConcurrentDictionary<string, MessageData> _messages
//            = new ConcurrentDictionary<string, MessageData>();

//        public MessageLoaderScope(IFileMessageService messageService, IFileUserService userService)
//        {
//            _messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
//            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
//        }

//        public async Task<UserData> GetUserData(string userId)
//        {
//            if (_users.TryGetValue(userId, out var user))
//            {
//                return user;
//            }
//            var data = await _userService.GetUserById(userId);
//            return _users.GetOrAdd(userId,
//                new UserData(data.Id, data.Id, data.IsOnline, data.LastChanged));
//        }

//        public async Task<MessageData> GetMessageData(string messageId)
//        {
//            if (_messages.TryGetValue(messageId, out var message))
//            {
//                return message;
//            }
//            var data = await _messageService.GetMessageById(messageId);
//            return _messages.GetOrAdd(messageId,
//                new MessageData(data.Id,
//                    data.SenderId,
//                    data.DateTime,
//                    data.IsOutgoing,
//                    new MessageTextContent(data.Text),
//                    data.ReplyToMessageId));
//        }

//        public void Dispose()
//        {
//            _users.Clear();
//            _messages.Clear();
//        }
//    }
//}
