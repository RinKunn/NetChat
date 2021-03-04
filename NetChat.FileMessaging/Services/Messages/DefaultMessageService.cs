using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NetChat.FileMessaging.Models;
using NetChat.FileMessaging.Repository.Messages;

namespace NetChat.FileMessaging.Services.Messages
{
    public class DefaultMessageService : IMessageService
    {
        private readonly IMessageDataEntityRepository _messageRepository;

        public DefaultMessageService(IMessageDataEntityRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task<IList<MessageData>> LoadMessagesAsync(int limit = 0, CancellationToken token = default)
        {
            var messages = await _messageRepository.GetAsync(limit, token);
            if (messages == null || messages.Count == 0)
                return null;
            List<MessageData> res = new List<MessageData>();
            for(int i = 0; i < messages.Count; i++)
            {
                MessageDataEntity m = messages[i];
                string text = m.Text;
                token.ThrowIfCancellationRequested();
                if(i + 1 < messages.Count && messages[i + 1].Id == m.Id)
                {
                    StringBuilder textBuilder = new StringBuilder(m.Text);
                    while(i + 1 < messages.Count && messages[i + 1].Id == m.Id)
                    {
                        textBuilder.Append("\n" + messages[++i].Text);
                    }
                    text = textBuilder.ToString();
                }
                res.Add(new MessageData(m.Id, m.DateTime,
                        m.UserName, text, m.ReplyToMessageId));
            }
            return res;
        }

        public async Task<MessageData> SendMessage(InputMessageData inputMessageData)
        {
            MessageDataEntity messageEntity = null;
            if (inputMessageData.Text.Contains("\n"))
            {
                var messageLines = inputMessageData.Text.Split('\n');
                var messages = messageLines
                    .Select(t => 
                        new MessageDataEntity(
                            inputMessageData.SenderId,
                            t, 
                            inputMessageData.DateTime, 
                            inputMessageData.ReplyToMessageId))
                    .ToList();
                messageEntity =
                    (await _messageRepository.AddSomeAsync(messages))
                    .First(m => m != null);
            }
            else
            {
                messageEntity = 
                    (await _messageRepository.AddAsync(
                        new MessageDataEntity(
                            inputMessageData.SenderId,
                            inputMessageData.Text,
                            inputMessageData.DateTime,
                            inputMessageData.ReplyToMessageId)));
            }
            return new MessageData(messageEntity.Id,
                messageEntity.DateTime,
                messageEntity.UserName,
                inputMessageData.Text,
                inputMessageData.ReplyToMessageId);
        }
    }
}
