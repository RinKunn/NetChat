using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using NetChat.Desktop.InnerMessages;
using NetChat.Desktop.ViewModel.Commands;
using NetChat.Desktop.ViewModel.Messenger.ChatArea.Factories;
using NetChat.Desktop.ViewModel.Messenger.ChatArea.Messages;
using NetChat.Services.Messaging.Chats;
using NetChat.Services.Messaging.Messages;
using NLog;

namespace NetChat.Desktop.ViewModel.Messenger.ChatArea
{

    public class ChatAreaViewModel : ViewModelBase
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly IChatLoader _chatLoader;
        private readonly IMessageLoader _messageLoader;
        private readonly IMessageUpdater _messageUpdater;
        private readonly IMessenger _innerMessageBus;
        private readonly IMessageFactory _messageFactory;
        
        private ObservableCollection<MessageObservable> _messages;
        private int _lastVisibleMessageIndex;
        private int _targetMessageIndex;
        private int _unreadMessagesCount;
        private int _lastReadedMessageDistance;

        private bool _isLoaded;
        public bool IsLoaded
        {
            get => _isLoaded;
            set => Set(ref _isLoaded, value);
        }

        private bool _hasLoadingError;
        public bool HasLoadingError
        {
            get => _hasLoadingError;
            set => Set(ref _hasLoadingError, value);
        }

        public int LastVisibleMessageIndex
        {
            get => _lastVisibleMessageIndex;
            set
            {
                Set(ref _lastVisibleMessageIndex, value);
                UnreadMessagesCount = 
                    Math.Min(UnreadMessagesCount,
                    _messages.Count - 1 - _lastVisibleMessageIndex);
                LastReadedMessageDistance =
                    _messages.Count - UnreadMessagesCount - 1 - value;
                GoToLastMessageCommand.RaiseCanExecuteChanged();
            }
        }

        public int TargetMessageIndex
        {
            get => _targetMessageIndex;
            set
            {
                Set(ref _targetMessageIndex, value);
                Set(ref _targetMessageIndex, -1);
            }
        }
        public int UnreadMessagesCount
        {
            get => _unreadMessagesCount;
            set => Set(ref _unreadMessagesCount, value);
        }

        public int LastReadedMessageDistance
        {
            get => _lastReadedMessageDistance;
            set => Set(ref _lastReadedMessageDistance, value);
        }

        public ObservableCollection<MessageObservable> Messages
        {
            get => _messages;
            private set => Set(ref _messages, value);
        }

#if DEBUG
        public ChatAreaViewModel()
        {
            if (IsInDesignMode)
            {
                _messages = new ObservableCollection<MessageObservable>
                {
                    new TextMessageObservable("1", DateTime.Now.AddMinutes(-3), 
                    "User 1", false, "Hello, cols", new ReplyObservable("12", "User2", "Hello reply message")),
                    new TextMessageObservable("2", DateTime.Now.AddMinutes(-2), 
                    "User Me", true, "Hello, User 1", new ReplyObservable("12", "User2", "Hello reply message")),
                    new TextMessageObservable("3", DateTime.Now.AddMinutes(-1),
                    "User 1", false, "Hello, User1 and User2, asdsadasdddddd dddddddddddddd ddddddddddddd ddddd")
                };
                UnreadMessagesCount = 0;
                LastReadedMessageDistance = 2;
                HasLoadingError = false;
                IsLoaded = true;
            }
            else throw new NotImplementedException("ChatArea without services is not implemented");
        }
#endif

        public ChatAreaViewModel(IChatLoader chatLoader, 
            IMessageLoader messageLoader,
            IMessageUpdater messageUpdater, 
            IMessenger innerMessageBus, 
            IMessageFactory messageFactory)
        {
            _messages = new ObservableCollection<MessageObservable>();
            _chatLoader = chatLoader ?? throw new ArgumentNullException(nameof(chatLoader));
            _messageLoader = messageLoader ?? throw new ArgumentNullException(nameof(messageLoader));
            _messageUpdater = messageUpdater ?? throw new ArgumentNullException(nameof(messageUpdater));
            _innerMessageBus = innerMessageBus ?? throw new ArgumentNullException(nameof(innerMessageBus));
            _messageFactory = messageFactory ?? throw new ArgumentNullException(nameof(messageFactory));

            _messageUpdater.Register(this, MessageReceivedHandler);
            _innerMessageBus.Register<GoToMessageIM>(this, (m) => GoToMessageById(m.MessageId));

            HasLoadingError = false;
            IsLoaded = false;
        }

        public override void Cleanup()
        {
            _logger.Debug("ViewModel cleaning up: '{0}'", GetType().Name);
            _innerMessageBus.Unregister<GoToMessageIM>(this);
            _messageUpdater.Unregister(this);

            string lastId = Messages[LastVisibleMessageIndex].MessageId;
            _logger.Debug("Saving ChatData info: LastReadMessageId='{0}'", lastId);
            _chatLoader.SetChatData(new ChatData(lastId));
            _messages.Clear();
            base.Cleanup();
            _logger.Debug("ViewModel cleaned up: '{0}'", GetType().Name);
        }

        private void MessageReceivedHandler(Message message)
        {
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                Messages.Add(_messageFactory.ToObservable(message));
                _logger.Debug("Added new message (id='{0}', outgoing={1}) to Messages list (on bottom={2}), Unread={3}", 
                    message.MessageData.Id, 
                    message.MessageData.IsOutgoing, 
                    LastVisibleMessageIndex < Messages.Count - 1, 
                    UnreadMessagesCount);
                //if (message.MessageData.IsOutgoing)
                //{
                //    GoToMessageByIndex(_messages.Count - 1);
                //}
                //else 
                if (LastVisibleMessageIndex < Messages.Count - 1)
                {
                    UnreadMessagesCount++;
                }
            });
        }

        private void GoToMessageById(string messageId)
        {
            GoToMessageByIndex(MessageIndexById(messageId));
        }

        private AsyncCommand _loadInitMessagesCommand;
        public AsyncCommand LoadMessagesCommand => _loadInitMessagesCommand ??
            (_loadInitMessagesCommand = new AsyncCommand(LoadInitMessagesAsync, (o) => !IsLoaded));
        private async Task LoadInitMessagesAsync()
        {
            _logger.Debug("Loading chat data: IsLoaded={0}, HasError={1}", IsLoaded, HasLoadingError);
            if (IsLoaded)
            {
                GoToMessageByIndex(Messages.Count - 1);
                return;
            }
            if (HasLoadingError) HasLoadingError = false;

            IList<Message> loadedMessages = null;
            try
            {
                var chatData = _chatLoader.GetChatData();
                string lastReadMessageId = chatData.LastReadInboxMessageId;
                _logger.Debug("Last read message id='{0}'", lastReadMessageId);

                _logger.Debug("Loading init messages from MessageId='{0}'...");
                loadedMessages = await _messageLoader.LoadMessagesAsync(null);
            }
            catch(Exception e)
            {
                _logger.Error("On initing: {0}", e.Message);
                _innerMessageBus.Send(new ExceptionIM(e, false));
                HasLoadingError = true;
                IsLoaded = false;
                throw e;
            }
            Messages = new ObservableCollection<MessageObservable>(
                loadedMessages?.Select(m => _messageFactory.ToObservable(m)));
            IsLoaded = true;
            _logger.Debug("Loaded {0} init messages", Messages.Count);
        }

        private RelayCommand _goToLastMessageCommand;
        public RelayCommand GoToLastMessageCommand => _goToLastMessageCommand ??
            (_goToLastMessageCommand 
            = new RelayCommand(GoToLastMessageHandler, CanExecuteGoToLastMessage));
        private void GoToLastMessageHandler()
        {
            _logger.Debug("GoToLast message clicked: Unread={0}, Distance={1}", UnreadMessagesCount, LastReadedMessageDistance);
            int index = -1;
            if(LastReadedMessageDistance > 0)
            {
                index = Messages.Count - UnreadMessagesCount - 1;
                _logger.Debug("Brang into view Last readed message");
                GoToMessageByIndex(index);
                return;
            }
            if (UnreadMessagesCount > 0)
                GoToMessageByIndex(Messages.Count - 1);
        }
        private bool CanExecuteGoToLastMessage()
        {
            return LastReadedMessageDistance > 0 || UnreadMessagesCount > 0;
        }

        private RelayCommand<MessageObservable> _replyToMessageCommand;
        public RelayCommand<MessageObservable> ReplyToMessageCommand
            => _replyToMessageCommand ??
            (_replyToMessageCommand = new RelayCommand<MessageObservable>(ReplyToMessageCommandHandle));
        private void ReplyToMessageCommandHandle(MessageObservable message)
        {
            _logger.Debug("Reply to Message (id='{0}') is clicked", message.MessageId);
            string text = "Unsupported message";
            if (message is TextMessageObservable textMessage)
                text = textMessage.Text;

            _innerMessageBus.Send(
                new ReplyToMessageIM(
                    message.MessageId,
                    message.AuthorName,
                    text));
        }

        private RelayCommand<MessageObservable> _goToReplyMessageCommand;
        public RelayCommand<MessageObservable> GoToReplyMessageCommand =>
            _goToReplyMessageCommand ??
            (_goToReplyMessageCommand 
                = new RelayCommand<MessageObservable>(
                    GoToReplyMessageCommandHandler));
        private void GoToReplyMessageCommandHandler(
            MessageObservable messageObservable)
        {
            if (!messageObservable.HasReply) return;
            _logger.Debug("Goto Replying message (id='{0}') is clicked", messageObservable.Reply.MessageId);
            GoToMessageById(messageObservable.Reply.MessageId);
        }


        private int MessageIndexById(string id)
        {
            var message = Messages.FirstOrDefault(m => m.MessageId == id);
            if (message == null)
                throw new ArgumentNullException($"Cannot find message by id: '{id}'");
            return Messages.IndexOf(message);
        }

        private void GoToMessageByIndex(int index)
        {
            _logger.Debug("Go to message index: {0} from {1}", index, Messages.Count - 1);
            if (index < 0 || index >= Messages.Count)
                throw new ArgumentOutOfRangeException($"Index '{index}' out of range [0; {Messages.Count}]");
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                int lastUnreadMessageIndex = Messages.Count - UnreadMessagesCount;
                if (UnreadMessagesCount > 0 && index > lastUnreadMessageIndex)
                {
                    for (int i = lastUnreadMessageIndex; i < index; i++)
                        TargetMessageIndex = i;
                }
                TargetMessageIndex = index;
            });
        }
    }
}
