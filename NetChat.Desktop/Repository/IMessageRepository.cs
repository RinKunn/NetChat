using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat.Desktop.Repository
{
    public interface IMessageRepository
    {
        Task SendMessage(MessageData messageData);
        Task<IList<MessageData>> GetMessages(int limit = 0);
    }
}
