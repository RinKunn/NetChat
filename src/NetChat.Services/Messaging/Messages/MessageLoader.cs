//using System.Collections.Generic;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;
//using NetChat.Services.Caching;
//using NetChat.Services.Mappers;
//using NetChat.Services.Messaging.Messages.Storage;
//using NetChat.Services.Messaging.Users;
//using NetChat.Services.Messaging.Users.Storage;
//using NLog;

//namespace NetChat.Services.Messaging.Messages
//{
//    public class MessageLoader : IMessageLoader
//    {
//        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
//        private readonly IDataCache _cache;
//        private readonly IUserDataRepository _userDataRepository;
//        private readonly IMdeRepository _messageDataRepository;
//        private readonly IMessageMapper _messageDataEntityMapper;


//        public async Task<IList<Message>> GetChatHistoryAsync(int limit, CancellationToken token)
//        {
//            var messagesData = await _messageDataRepository.Get(limit * 2, token);
//            List<Message> res = new List<Message>();
//            int messageLimit = limit;
//            for (int i = messagesData.Count - 1; i >= 0 && messageLimit > 0; i--)
//            {
//                MessageData messageData = _messageDataEntityMapper.ToMessageData(messagesData[i]);

//                UserData userData = await _cache.GetOrCreate<UserData>(
//                    messagesData[i].UserName,
//                    async () => await _userDataRepository.GetByUserName(messagesData[i].UserName),
//                    CachePolicies.UserDataPolicy);

                

//            }
//        }


//    }
//}

////namespace NetChat.Services.Messaging.Messages
////{
////    public class MessageLoader : IMessageLoader
////    {
////        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
////        private readonly IFileMessageService _messageService;
////        private readonly IFileUserService _userService;

////        public MessageLoader(IFileMessageService messageService, IFileUserService userService)
////        {
////            _messageService = messageService;
////            _userService = userService;
////        }

////        public async Task<IList<Message>> LoadMessagesAsync(int limit = 0)
////        {
////            IList<Message> res = null;
////            using (var scope = new MessageLoaderScope(_messageService, _userService))
////            {
////                _logger.Debug("Loading messages...");
////                var messages = await _messageService.LoadMessagesAsync(limit);
////                if (messages == null)
////                {
////                    _logger.Warn("Loaded message's list is empty");
////                    return null;
////                }
////                _logger.Debug("Loaded {0} messages", messages.Count);
////                _logger.Debug("Mapping messages...");
////                res = new List<Message>(
////                    await Task.WhenAll(messages
////                    .Select(async m
////                        => await MapToMessage(scope,
////                                new MessageData(m.Id, m.SenderId, m.DateTime,

////                                m.IsOutgoing,
////                                    new MessageTextContent(m.Text),
////                                    m.ReplyToMessageId)))));
////                _logger.Debug("Loaded messages mapped successfull");
////            }
////            return res;
////        }

////        private async Task<Message> MapToMessage(MessageLoaderScope scope, MessageData messageData)
////        {
////            UserData userData = await scope.GetUserData(messageData.SenderId);
////            if (userData == null)
////            {
////                _logger.Error("There is no userdata for UserId = {0}", messageData.SenderId);
////                userData = new UserData(messageData.SenderId,
////                    messageData.SenderId,
////                    true,
////                    messageData.Date);
////            }

////            Message replyMessage = null;
////            if (!string.IsNullOrEmpty(messageData.ReplyToMessageId))
////            {
////                var replyMessageData = await scope.GetMessageData(messageData.ReplyToMessageId);
////                if (replyMessageData == null)
////                {
////                    _logger.Error("There is no messagedata for MessageId = {0}", messageData.ReplyToMessageId);
////                }
////                replyMessage = await MapToMessage(scope, replyMessageData);
////            }
////            return new Message(messageData, userData, replyMessage);
////        }
////    }
////}
