using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NetChat.Services.FileMessaging.Data;

namespace NetChat.Services.FileMessaging.Repository
{
    public interface IMessageDataRepository
    {
        Task AddAsync(MessageDataEntity messageData);
        Task<IList<MessageDataEntity>> GetSomeAsync(int fileOffset, int limit, CancellationToken token = default);
        Task<IList<MessageDataEntity>> GetBy(int limit, CancellationToken token = default);
    }
}
