using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using NetChat.Desktop.Services.Messaging.Messages;

namespace NetChat.Desktop.Repository
{
    public class MessageRepository : IMessageRepository
    {
        private readonly string _filePath;
        private readonly Encoding _encoding;

        public MessageRepository(RepositoryConfigs repositoryConfigs)
        {
            _filePath = repositoryConfigs.MessagesSourcePath;
            _encoding = repositoryConfigs.MessagesSourceEncoding;
        }

        public async Task<IList<MessageData>> GetMessages(int limit = 0)
        {
            var lines = await FileHelper.GetStringMessagesAsync(_filePath, _encoding, limit);
            return lines
                .Select(l => new NetChatMessage(l))
                .Where(m => m.Text != "Logon" && m.Text != "Logout")
                .Select(m => new MessageTextData()
                {
                    Id = m.Id,
                    DateTime = m.DateTime,
                    UserId = m.UserName,
                    Text = m.Text
                })
                .ToArray();
        }

        public async Task SendMessage(MessageData messageData)
        {
            if(messageData is MessageTextData inputMessageText)
            {
                var message = new NetChatMessage(inputMessageText.UserId, inputMessageText.Text, inputMessageText.DateTime);
                await Task.Run(() => File.AppendAllText(_filePath, message.ToString() + '\n', _encoding));
            }
        }
    }
}
