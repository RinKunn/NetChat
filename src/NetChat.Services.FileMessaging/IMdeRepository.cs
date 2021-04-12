using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetChat.Services.FileMessaging
{
    public interface IMdeRepository
    {
        Task AddAsync(MessageDataEntity messageData);
        Task AddSomeAsync(IEnumerable<MessageDataEntity> messagesData);

        Task<IList<MessageDataEntity>> Get(string fromMessageId, int offset, int limit, CancellationToken token = default);
        Task<IList<MessageDataEntity>> Get(int limit, CancellationToken token = default);

        Task<MessageDataEntity> GetById(string messageId, CancellationToken token = default);
        IList<MessageDataEntity> GetMessagesFromId(string messageId);
    }
}
