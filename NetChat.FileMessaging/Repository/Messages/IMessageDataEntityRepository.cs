using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetChat.FileMessaging.Repository.Messages
{
    public interface IMessageDataEntityRepository
    {
        Task<MessageDataEntity> AddAsync(MessageDataEntity messageData);
        Task<IEnumerable<MessageDataEntity>> AddSomeAsync(IEnumerable<MessageDataEntity> messageDatas);
        Task<IList<MessageDataEntity>> GetAsync(int limit, CancellationToken token = default);
        Task<IList<MessageDataEntity>> GetAllAsync(CancellationToken token = default);
    }
}
