//using System;
//using System.Threading.Tasks;
//using NetChat.FileMessaging.Models;
//using NetChat.FileMessaging.Services.Messages;
//using NLog;

//namespace NetChat.Services.Messaging.Messages
//{
//    public class MessageSender : IMessageSender
//    {
//        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
//        private readonly IFileMessageService _messageService;

//        public MessageSender(IFileMessageService messageService)
//        {
//            _messageService = messageService;
//        }

//        public async Task<MessageData> SendMessage(InputMessageContent content, string replyToMessageId)
//        {
//            MessageData res = null;
//            if (content is InputMessageTextContent mt)
//            {
//                _logger.Debug("Sending a message: Text='{0}'", mt.Text);
//                try
//                {
//                    var data = await _messageService.SendMessage(new InputMessageData(mt.Text, replyToMessageId));
//                    _logger.Debug("Message sent successfully");
//                    res = new MessageData(data.Id,
//                        data.SenderId, data.DateTime, true,
//                        new MessageTextContent(data.Text),
//                        data.ReplyToMessageId);
//                }
//                catch (Exception e)
//                {
//                    _logger.Error($"Error sending message: Error='{0}', InnerError='{1}'",
//                        e.Message, e.InnerException?.Message);
//                    res = null;
//                }
//            }
//            else throw new NotImplementedException(content.GetType().Name);
//            return res;
//        }
//    }
//}
