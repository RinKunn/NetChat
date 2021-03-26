//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using NetChat.FileMessaging.Services.Messages;
//using NetChat.FileMessaging.Services.Users;
//using NLog;

//namespace NetChat.Services.Messaging.Messages
//{
//    public class MessageLoader : IMessageLoader
//    {
//        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
//        private readonly IFileMessageService _messageService;
//        private readonly IFileUserService _userService;

//        public MessageLoader(IFileMessageService messageService, IFileUserService userService)
//        {
//            _messageService = messageService;
//            _userService = userService;
//        }

//        public async Task<IList<Message>> LoadMessagesAsync(int limit = 0)
//        {
//            IList<Message> res = null;
//            using (var scope = new MessageLoaderScope(_messageService, _userService))
//            {
//                _logger.Debug("Loading messages...");
//                var messages = await _messageService.LoadMessagesAsync(limit);
//                if (messages == null)
//                {
//                    _logger.Warn("Loaded message's list is empty");
//                    return null;
//                }
//                _logger.Debug("Loaded {0} messages", messages.Count);
//                _logger.Debug("Mapping messages...");
//                res = new List<Message>(
//                    await Task.WhenAll(messages
//                    .Select(async m
//                        => await MapToMessage(scope,
//                                new MessageData(m.Id, m.SenderId, m.DateTime,

//                                m.IsOutgoing,
//                                    new MessageTextContent(m.Text),
//                                    m.ReplyToMessageId)))));
//                _logger.Debug("Loaded messages mapped successfull");
//            }
//            return res;
//        }

//        private async Task<Message> MapToMessage(MessageLoaderScope scope, MessageData messageData)
//        {
//            UserData userData = await scope.GetUserData(messageData.SenderId);
//            if (userData == null)
//            {
//                _logger.Error("There is no userdata for UserId = {0}", messageData.SenderId);
//                userData = new UserData(messageData.SenderId,
//                    messageData.SenderId,
//                    true,
//                    messageData.Date);
//            }

//            Message replyMessage = null;
//            if (!string.IsNullOrEmpty(messageData.ReplyToMessageId))
//            {
//                var replyMessageData = await scope.GetMessageData(messageData.ReplyToMessageId);
//                if (replyMessageData == null)
//                {
//                    _logger.Error("There is no messagedata for MessageId = {0}", messageData.ReplyToMessageId);
//                }
//                replyMessage = await MapToMessage(scope, replyMessageData);
//            }
//            return new Message(messageData, userData, replyMessage);
//        }
//    }
//}
