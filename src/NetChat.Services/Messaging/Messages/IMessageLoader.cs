using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NetChat.Services.Messaging.Messages
{
    public interface IMessageLoader
    {
        Task<IList<Message>> GetChatHistoryAsync(int limit, CancellationToken token);

        //TODO add virtualisation
    }
}
