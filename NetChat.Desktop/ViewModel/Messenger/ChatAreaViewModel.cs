using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Threading;
using NetChat.Desktop.Services.Messaging;
using NetChat.Desktop.Services.Messaging.Messages;
using NetChat.Desktop.ViewModel.Commands;
using NLog;

namespace NetChat.Desktop.ViewModel.Messenger
{
    public class ChatAreaViewModel : ViewModelBase
    {
        private IMessageLoader _messageLoader;
        private IReceiverHub _receiverHub;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

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
                Messages.Add(new TextMessageObservable("Hello, cols", "1", DateTime.Now.AddMinutes(-3), new ParticipantObservable("User1", true, DateTime.Now.AddHours(-1))));
                Messages.Add(new TextMessageObservable("Hello, User1", "2", DateTime.Now.AddMinutes(-2), new ParticipantObservable("User2", true, DateTime.Now.AddHours(-2)), true));
                Messages.Add(new TextMessageObservable("Hello, User1 and User2, asdsadasdddddd dddddddddddddd ddddddddddddd ddddd", "3", DateTime.Now.AddMinutes(-1), new ParticipantObservable("User3", true, DateTime.Now.AddHours(-3))));
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

        private void HandleMessage(MessageObservable message)
        {
            DispatcherHelper.CheckBeginInvokeOnUI(() => Messages.Add(message));
        }

        private IAsyncCommand _loadMessagesCommand;
        public IAsyncCommand LoadMessagesCommand => _loadMessagesCommand ??
            (_loadMessagesCommand = new AsyncCommand(LoadMessages));

        private async Task LoadMessages()
        {
            _logger.Info("Hub: {0}, {1}", _receiverHub.IsConnected, _receiverHub.GetHashCode());
            var loadedMessages = await _messageLoader.LoadMessagesAsync();
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
                Messages = new ObservableCollection<MessageObservable>(loadedMessages));
        }
    }
}
