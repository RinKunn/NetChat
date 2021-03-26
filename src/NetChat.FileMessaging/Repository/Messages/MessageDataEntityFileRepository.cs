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
    public class MessageDataEntityFileRepository : IMessageDataEntityRepository
    {
        private readonly string _filename;
        private readonly Encoding _encoding;

        public MessageDataEntityFileRepository(RepositoriesConfig config)
        {
            _filename = config.MessagesSourcePath;
            _encoding = config.MessagesSourceEncoding;
        }


        public async Task<MessageDataEntity> AddAsync(MessageDataEntity messageData)
        {
            await Task.Run(() 
                => File.AppendAllText(
                    _filename, 
                    messageData.ToString() + '\n', 
                    _encoding));
            return messageData;
        }

        public async Task<IEnumerable<MessageDataEntity>> AddSomeAsync(IEnumerable<MessageDataEntity> messageDatas)
        {
            await Task.Run(() 
                => File.AppendAllText(
                    _filename,
                    string.Concat(messageDatas.Select(m => m.ToString() + "\n")),
                    _encoding));
            return messageDatas;
        }

        public async Task<IList<MessageDataEntity>> GetAllAsync(CancellationToken token = default)
        {
            return await GetAsync(0, token);
        }

        public async Task<IList<MessageDataEntity>> GetAsync(int limit, CancellationToken token = default)
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

        //public async Task<IList<TextMessageData>> GetAsync(int limit, CancellationToken token = default)
        //{
        //    if (limit < 0) throw new ArgumentNullException(nameof(limit));
        //    var res = await Task.Run(() =>
        //    {
        //        var lines = File.ReadAllLines(_filename, _encoding);
        //        return lines
        //            .Skip(lines.Length <= limit || limit == 0 ? 0 : lines.Length - limit)
        //            .Select(l => new TextMessageData(l))
        //            .Where(m => m.Text != "Logon" && m.Text != "Logout")
        //            .ToArray();
        //    }, token);
        //    return res;
        //}
    }
}
