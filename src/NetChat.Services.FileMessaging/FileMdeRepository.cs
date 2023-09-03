using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetChat.Services.FileMessaging
{
    public class FileMdeRepository : IMdeRepository
    {
        private readonly string _filename;
        private readonly Encoding _encoding;

        public FileMdeRepository(MdeConfig config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));
            if (string.IsNullOrEmpty(config.MessagesSourcePath))
                throw new ArgumentNullException(nameof(config.MessagesSourcePath));
            _filename = config.MessagesSourcePath;
            _encoding = config.MessagesSourceEncoding
                ?? throw new ArgumentNullException(nameof(config.MessagesSourcePath));
        }

        public Task AddAsync(MessageDataEntity messageData)
        {
            throw new NotImplementedException();
        }

        public Task AddSomeAsync(IEnumerable<MessageDataEntity> messagesData)
        {
            throw new NotImplementedException();
        }

        public Task<IList<MessageDataEntity>> Get(int fileOffset, int limit, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<IList<MessageDataEntity>> Get(string fromMessageId, int offset, int limit, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<IList<MessageDataEntity>> Get(int limit, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<MessageDataEntity> GetById(string messageId, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public IList<MessageDataEntity> GetMessagesFromId(string messageId)
        {
            throw new NotImplementedException();
        }


        /*TODO
         * List<MDE> -> List<MD>
         * List<MDE> -> List<UD>
         * On cache need MD, UD, UStatus
         * 
         * On init need add to cache list init messages to avoid second reading
         * Hub indicate only changes, UUpdater and MUpdater only subscribe to hub
         */
    }
}
