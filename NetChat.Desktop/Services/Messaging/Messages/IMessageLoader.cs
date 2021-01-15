using System.Collections.Generic;
using System.Threading.Tasks;
using NetChat.Desktop.ViewModel.Messenger;

namespace NetChat.Desktop.Services.Messaging.Messages
{
    public interface IMessageLoader
    {
        Task<IList<MessageObservable>> LoadMessagesAsync(int limit = 0);
    }
}
