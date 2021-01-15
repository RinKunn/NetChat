using System;
using System.Threading.Tasks;
using NetChat.FileMessaging.Models;
using NetChat.FileMessaging.Services.Messages;

namespace NetChat.Desktop.Services.Messaging.Messages
{
    public class MessageSender : IMessageSender
    {
        private readonly IMessageService _messageService;

        public MessageSender(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public async Task SendMessage(SendingMessage message)
        {
            if (message is SendingTextMessage mt)
                await _messageService.SendMessage(new InputTextMessage(mt.UserId, mt.Text));
            else throw new NotImplementedException(message.GetType().Name);
        }
    }
}
