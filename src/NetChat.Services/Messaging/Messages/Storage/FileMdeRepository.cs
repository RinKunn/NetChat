using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NetChat.Services.FileMessaging.Repository;

namespace NetChat.Services.Messaging.Messages.Storage
{
    public class FileMdeRepository : IMdeRepository
    {
        private readonly string _filename;
        private readonly Encoding _encoding;

        public FileMdeRepository(RepositoryConfig config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));
            if (string.IsNullOrEmpty(config.MessagesSourcePath))
                throw new ArgumentNullException(nameof(config.MessagesSourcePath));
            _filename = config.MessagesSourcePath;
            _encoding = config.MessagesSourceEncoding
                ?? throw new ArgumentNullException(nameof(config.MessagesSourcePath));
        }

        public async Task AddAsync(MessageDataEntity messageData)
        {
            await Task.Run(()
                => File.AppendAllText(
                        _filename,
                        messageData.ToString() + '\n',
                        _encoding));
        }

        /*TODO
         * List<MDE> -> List<MD>
         * List<MDE> -> List<UD>
         * On cache need MD, UD, UStatus
         * 
         * On init need add to cache list init messages to avoid second reading
         * Hub indicate only changes, UUpdater and MUpdater only subscribe to hub
         */

        public Task<IList<MessageDataEntity>> GetBy(int limit, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<IList<MessageDataEntity>> GetSomeAsync(int fileOffset, int limit, CancellationToken token = default)
        {
            if (limit < 0) throw new ArgumentNullException(nameof(limit));
            var lines = await FileHelper.GetStringMessagesAsync(_filename, _encoding, 0, token);
            Console.WriteLine("readed: {0}", lines.Length);
            return lines
                    .Skip(lines.Length <= limit || limit == 0 ? 0 : lines.Length - limit)
                    .Select(l => MessageDataEntity.ParseOrDefault(l))
                    .Where(m => m != null && m.Text != "Logon" && m.Text != "Logout")
                    .ToArray();
        }
    }
}
