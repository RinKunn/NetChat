using System;
using System.Threading.Tasks;
using NetChat.FileMessaging.Models;
using NetChat.FileMessaging.Services.Messages;
using NLog;

namespace NetChat.Desktop.Services.Messaging.Messages
{
    public class MessageSender : IMessageSender
    {
        private readonly IMessageService _messageService;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public MessageSender(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public async Task SendMessage(SendingMessage message)
        {
            if (message is SendingTextMessage mt)
            {
                _logger.Debug("User '{0}' send message: {1}", mt.UserId, mt.Text);
                await _messageService.SendMessage(new InputTextMessage(mt.UserId, mt.Text));
            }
            else throw new NotImplementedException(message.GetType().Name);
        }
    }
}
