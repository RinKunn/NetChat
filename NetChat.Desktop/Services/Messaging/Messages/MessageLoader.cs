using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetChat.Desktop.Repository;
using NetChat.Desktop.Services.Messaging.Users;

namespace NetChat.Desktop.Services.Messaging.Messages
{
    public class MessageLoader : IMessageLoader
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IUserLoader _userLoader;

        public MessageLoader(IMessageRepository messageRepository, IUserLoader userLoader)
        {
            _messageRepository = messageRepository;
            _userLoader = userLoader;
        }

        public async Task<IEnumerable<Message>> LoadMessagesAsync(int limit = 0)
        {
            var messages = await _messageRepository.GetMessages(limit);
            return
                await Task.WhenAll(
                    messages
                    .Select(async m =>
                    {
                        var user = await _userLoader.GetUserById(m.UserId);
                        return m is MessageTextData messageTextData
                            ? new MessageText()
                            {
                                Id = messageTextData.Id,
                                DateTime = messageTextData.DateTime,
                                Text = messageTextData.Text,
                                Sender = user,
                                IsOriginNative = _userLoader.IsMe(messageTextData.UserId)
                            }
                            : new MessageText();
                    }));
        }
    }
}
