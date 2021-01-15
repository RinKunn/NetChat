using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetChat.FileMessaging.Repository.Messages
{
    public interface ITextMessageDataRepository
    {
        Task AddAsync(TextMessageData messageData);
        Task<IList<TextMessageData>> GetAsync(int limit, CancellationToken token = default);
        Task<IList<TextMessageData>> GetAllAsync(CancellationToken token = default);
    }
}
