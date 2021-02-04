using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
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

        private void Messages_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if(e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                if(((MessageObservable)e.NewItems[e.NewItems.Count - 1]).IsOriginNative)
                {
                    ReadAllMessages();
                }
                else if (LastVisibleMessageIndex < Messages.Count - 1)
                    NewMessagesCount += e.NewItems.Count;
            }
            
            
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
        }

        public override void Cleanup()
        {
            _receiverHub.UnsubscribeMessageReceived(this);
            base.Cleanup();
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
        }

        private ICommand _readAllMessagesCommand;
        public ICommand ReadAllMessagesCommand => _readAllMessagesCommand ??
            (_readAllMessagesCommand = new RelayCommand(ReadAllMessages));

        private void ReadAllMessages()
        {
            LastVisibleMessageIndex = _messages.Count - 1;
        }

            
    }
}
