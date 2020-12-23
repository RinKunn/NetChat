using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat.Desktop.Services.Messaging.Messages
{
    public interface IMessageLoader
    {
        Task<IEnumerable<Message>> LoadMessagesAsync(int limit = 0);
    }

    public class DefaultMessageLoader : IMessageLoader
    {
        public async Task<IEnumerable<Message>> LoadMessagesAsync(int limit = 0)
        {
            await Task.Delay(2000).ConfigureAwait(false);
            var messages = new List<Message>();
            return messages;
        }
    }
}
