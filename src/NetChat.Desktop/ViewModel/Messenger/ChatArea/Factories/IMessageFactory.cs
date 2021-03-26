using NetChat.Desktop.ViewModel.Messenger.ChatArea.Messages;
using NetChat.Services.Messaging.Messages;

namespace NetChat.Desktop.ViewModel.Messenger.ChatArea.Factories
{
    public interface IMessageFactory
    {
        MessageObservable ToObservable(Message message);
    }
}
