using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat.Desktop.Services.Messaging.Messages
{
    public interface IMessageSender
    {
        Task SendMessage(InputMessage message);
    }

    public class DefaultMessageSender : IMessageSender
    {
        public async Task SendMessage(InputMessage message)
        {
            await Task.Delay(3000).ConfigureAwait(false);
        }
    }
}
