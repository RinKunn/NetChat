using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NetChat.FileMessaging.Models;
using NetChat.FileMessaging.Repository.Messages;

namespace NetChat.FileMessaging.Services.Messages
{
    public class DefaultMessageService : IMessageService
    {
        private readonly ITextMessageDataRepository _messageRepository;

        public DefaultMessageService(ITextMessageDataRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task<IList<TextMessage>> LoadMessagesAsync(int limit = 0, CancellationToken token = default)
        {
            var messages = await _messageRepository.GetAsync(limit, token);
            return messages
                .Select(m => new TextMessage()
                {
                    Id = m.Id,
                    SenderId = m.UserName,
                    DateTime = m.DateTime,
                    Text = m.Text
                })
                .ToList();
        }

        public async Task SendMessage(InputTextMessage message)
        {
            await _messageRepository.AddAsync(new TextMessageData(message.UserId, message.Text, message.DateTime));
        }

    }
}
