using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetChat.Services.Messaging.Messages
{
    public interface IMessageLoader
    {
        Task<IList<Message>> LoadMessagesAsync(string fromMessId, int limit = 0);
        //TODO add virtualisation
    }
}
