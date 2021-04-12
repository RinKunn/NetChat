using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetChat.Services.Messaging.Messages.Storage
{
    public interface IMdeRepository
    {
        Task AddAsync(MessageDataEntity messageData);
        Task AddSomeAsync(IEnumerable<MessageDataEntity> messagesData);
        Task<IList<MessageDataEntity>> Get(int fileOffset, int limit, CancellationToken token = default);
        Task<MessageDataEntity> GetById(string messageId, CancellationToken token = default);

        int ReadLastFromIndex(int index, out IList<MessageDataEntity> entities);
    }
}
