using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NetChat.FileMessaging.Models;

namespace NetChat.FileMessaging.Services.Messages
{
    public interface IMessageService
    {
        Task<IEnumerable<TextMessage>> LoadMessagesAsync(int limit = 0, CancellationToken token = default);
        Task SendMessage(InputTextMessage message);
    }
}
