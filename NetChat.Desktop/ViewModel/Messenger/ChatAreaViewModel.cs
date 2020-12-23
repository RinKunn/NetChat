using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using NetChat.Desktop.ViewModel.Commands;
using NetChat.Desktop.Services.Messaging.Messages;
using Locator = CommonServiceLocator.ServiceLocator;
using NetChat.Desktop.Services.Messaging.Users;
using GalaSoft.MvvmLight.Threading;

namespace NetChat.Desktop.ViewModel.Messenger
{
    public class ChatAreaViewModel : ViewModelBase
    {
        private IMessageLoader _messageLoader;

        private ObservableCollection<MessageObservable> _messages;
        public ObservableCollection<MessageObservable> Messages
        {
            get => _messages;
            private set => Set(ref _messages, value);
        }

        public ChatAreaViewModel()
        {
            if(IsInDesignModeStatic)
            {
                Messages = new ObservableCollection<MessageObservable>();
                Messages.Add(new MessageTextObservable("Hello, cols", "1", DateTime.Now.AddMinutes(-3), new ParticipantObservable("User1", true, DateTime.Now.AddHours(-1))));
                Messages.Add(new MessageTextObservable("Hello, User1", "2", DateTime.Now.AddMinutes(-2), new ParticipantObservable("User2", true, DateTime.Now.AddHours(-2)), true));
                Messages.Add(new MessageTextObservable("Hello, User1 and User2, asdsadasdddddd dddddddddddddd ddddddddddddd ddddd", "3", DateTime.Now.AddMinutes(-1), new ParticipantObservable("User3", true, DateTime.Now.AddHours(-3))));
            }
        }

        public ChatAreaViewModel(IMessageLoader messageLoader)
        {
            _messageLoader = messageLoader;
            Messages = new ObservableCollection<MessageObservable>();
        }

        private IAsyncCommand _loadMessagesCommand;
        public IAsyncCommand LoadMessagesCommand => _loadMessagesCommand ??
            (_loadMessagesCommand = new AsyncCommand(LoadMessages));

        private async Task LoadMessages()
        {
            var loadedMessages = await _messageLoader.LoadMessagesAsync();
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
                Messages = new ObservableCollection<MessageObservable>(loadedMessages.Select(m => MessageToObservable(m))));
        }



        private ParticipantObservable UserToObservable(User user)
        {
            return new ParticipantObservable(user.Id, user.Status == UserStatus.Online, user.StatusChangedDateTime);
        }
        private MessageObservable MessageToObservable(Message message)
        {
            MessageObservable observMessage = null;
            switch (message)
            {
                case MessageText mt:
                    observMessage = new MessageTextObservable(
                        mt.Text,
                        mt.Id,
                        mt.DateTime,
                        UserToObservable(mt.Sender),
                        mt.IsOriginNative);
                        break;
                default:
                    break;
            }
            return observMessage;
        }
    }
}
