using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
            return await GetAsync(0, token);
        }

        public async Task<IList<TextMessageData>> GetAsync(int limit, CancellationToken token = default)
        {
            if (limit < 0) throw new ArgumentNullException(nameof(limit));
            var res = await Task<IList<TextMessageData>>.Run(() =>
            {
                var lines = File.ReadAllLines(_filename, _encoding);
                return lines
                    .Skip(lines.Length <= limit || limit == 0 ? 0 : lines.Length - limit)
                    .Select(l => new TextMessageData(l))
                    .Where(m => m.Text != "Logon" && m.Text != "Logout")
                    .ToArray();
            }, token);
            return res;
        }
    }
}
