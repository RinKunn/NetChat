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
using NetChat.Desktop.Services.Messaging.Users;
using NetChat.Desktop.Services.Messaging;
using GalaSoft.MvvmLight.Threading;

namespace NetChat.Desktop.ViewModel.Messenger
{
    public class ChatAreaViewModel : ViewModelBase
    {
        private IMessageLoader _messageLoader;
        private IReceiverHub _receiverHub;

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
            else throw new NotImplementedException();
        }

        public ChatAreaViewModel(IMessageLoader messageLoader, IReceiverHub receiverHub)
        {
            _messageLoader = messageLoader ?? throw new ArgumentNullException(nameof(messageLoader));
            _receiverHub = receiverHub ?? throw new ArgumentNullException(nameof(receiverHub));
            _receiverHub.SubscribeMessageReceived(this, HandleMessage);
            Messages = new ObservableCollection<MessageObservable>();
        }

        public override void Cleanup()
        {
            base.Cleanup();
            _receiverHub.UnsubscribeMessageReceived(this);
        }

        private void HandleMessage(Message message)
        {
            DispatcherHelper.CheckBeginInvokeOnUI(() => Messages.Add(MessageToObservable(message)));
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
            return new ParticipantObservable(user.Id, user.IsOnline, user.LastChanged);
        }

        private MessageObservable MessageToObservable(Message message)
        {
            MessageObservable observMessage = null;
            switch (message)
            {
                case TextMessage mt:
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
