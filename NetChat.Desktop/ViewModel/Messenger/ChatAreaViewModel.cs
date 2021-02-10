using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using NetChat.Desktop.Services.Messaging;
using NetChat.Desktop.Services.Messaging.Messages;
using NetChat.Desktop.ViewModel.Commands;
using NetChat.Desktop.ViewModel.InnerMessages;
using NLog;

namespace NetChat.Desktop.ViewModel.Messenger
{
    public class ChatAreaViewModel : ViewModelBase
    {
        private readonly IMessenger _innerCommunication;
        private readonly IMessageLoader _messageLoader;
        private readonly IReceiverHub _receiverHub;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        private int _lastVisibleMessageIndex;
        public int LastVisibleMessageIndex
        {
            get => _lastVisibleMessageIndex;
            set
            {
                Set(ref _lastVisibleMessageIndex, value);
                NewMessagesCount = Math.Min(NewMessagesCount, _messages.Count - 1 - _lastVisibleMessageIndex);
            }
        }

        private int _newMessagesCount;
        public int NewMessagesCount
        {
            get => _newMessagesCount;
            set => Set(ref _newMessagesCount, value);
        }

        private ObservableCollection<MessageObservable> _messages;
        public ObservableCollection<MessageObservable> Messages
        {
            get => _messages;
            private set
            {
                if(_messages != null) _messages.CollectionChanged -= Messages_CollectionChanged;
                Set(ref _messages, value);
                if (_messages != null) _messages.CollectionChanged += Messages_CollectionChanged;
            }
        }

#if DEBUG
        public ChatAreaViewModel()
        {
            if(IsInDesignMode)
            {
                Messages = new ObservableCollection<MessageObservable>
                {
                    new TextMessageObservable("Hello, cols", "1", DateTime.Now.AddMinutes(-3), new ParticipantObservable("User1", true, DateTime.Now.AddHours(-1))),
                    new TextMessageObservable("Hello, User1", "2", DateTime.Now.AddMinutes(-2), new ParticipantObservable("User2", true, DateTime.Now.AddHours(-2)), true),
                    new TextMessageObservable("Hello, User1 and User2, asdsadasdddddd dddddddddddddd ddddddddddddd ddddd", "3", DateTime.Now.AddMinutes(-1), new ParticipantObservable("User3", true, DateTime.Now.AddHours(-3)))
                };
            }
            else throw new NotImplementedException("ChatArea without services is not implemented");
        }
#endif

        public ChatAreaViewModel(IMessageLoader messageLoader, IReceiverHub receiverHub, IMessenger innerCommunication)
        {
            _messageLoader = messageLoader ?? throw new ArgumentNullException(nameof(messageLoader));
            _receiverHub = receiverHub ?? throw new ArgumentNullException(nameof(receiverHub));
            _innerCommunication = innerCommunication ?? throw new ArgumentNullException(nameof(innerCommunication));
            _receiverHub.SubscribeMessageReceived(this, HandleMessage);
            _innerCommunication.Register<GoToMessageInnerMessage>(this, (m) => FindMessage(m.MessageId));
        }

        public override void Cleanup()
        {
            _logger.Debug("ChatArea is cleaning");
            _innerCommunication.Unregister<GoToMessageInnerMessage>(this);
            _receiverHub.UnsubscribeMessageReceived(this);
            base.Cleanup();
        }

        private void Messages_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                if (((MessageObservable)e.NewItems[e.NewItems.Count - 1]).IsOriginNative)
                {
                    ReadAllMessages();
                }
                else if (LastVisibleMessageIndex < Messages.Count - 1)
                    NewMessagesCount += e.NewItems.Count;
            }
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
            var loadedMessages = await _messageLoader.LoadMessagesAsync();
            Messages = new ObservableCollection<MessageObservable>(loadedMessages);
            LastVisibleMessageIndex = Messages.Count - 1;
        }

        private ICommand _readAllMessagesCommand;
        public ICommand ReadAllMessagesCommand => _readAllMessagesCommand ??
            (_readAllMessagesCommand = new RelayCommand(ReadAllMessages));

        private void ReadAllMessages()
        {
            LastVisibleMessageIndex = _messages.Count - 1;
        }


        private void FindMessage(string id)
        {
            _logger.Debug("Searching message with id='{0}'", id);
            var message = Messages.First(m => m.Id == id);
            if(message != null) _logger.Debug("Message found");
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
                LastVisibleMessageIndex = Messages.IndexOf(message));
        }
    }
}
