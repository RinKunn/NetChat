using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NetChat.FileMessaging.Common;

namespace NetChat.FileMessaging.Repository.Messages
{
    public class TextMessageDataFileRepository : ITextMessageDataRepository
    {
        private readonly string _filename;
        private readonly Encoding _encoding;

        public TextMessageDataFileRepository(RepositoriesConfig config)
        {
            _filename = config.MessagesSourcePath;
            _encoding = config.MessagesSourceEncoding;
        }


        public async Task AddAsync(TextMessageData messageData)
        {
            await Task.Run(() => File.AppendAllText(_filename, messageData.ToString() + '\n', _encoding));
        }

        public async Task<IList<TextMessageData>> GetAllAsync(CancellationToken token = default)
        {
            var lines = await FileHelper.GetStringMessagesAsync(_filename, _encoding, 0, token);
            return lines
                .Select(l => new TextMessageData(l))
                .Where(m => m.Text != "Logon" && m.Text != "Logout")
                .ToArray();
        }

        public async Task<IList<TextMessageData>> GetAsync(int limit, CancellationToken token = default)
        {
            if (limit <= 0) throw new ArgumentNullException(nameof(limit));
            var lines = await FileHelper.GetStringMessagesAsync(_filename, _encoding, limit, token);
            return lines
                .Select(l => new TextMessageData(l))
                .Where(m => m.Text != "Logon" && m.Text != "Logout")
                .ToArray();
        }
    }
}
