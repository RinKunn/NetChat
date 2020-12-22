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
}
