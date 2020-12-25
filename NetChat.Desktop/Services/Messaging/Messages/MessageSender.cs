using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetChat.Desktop.Repository;

namespace NetChat.Desktop.Services.Messaging.Messages
{
    public class MessageSender : IMessageSender
    {
        private readonly IMessageRepository _messageRepository;

        public MessageSender(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task SendMessage(InputMessage message)
        {
            if (message is InputMessageText messageText)
                await _messageRepository.SendMessage(new MessageTextData()
                {
                     DateTime = messageText.DateTime,
                     UserId = messageText.UserId,
                     Text = messageText.Text
                });
            else throw new NotImplementedException(message.GetType().Name);
        }
    }
}
