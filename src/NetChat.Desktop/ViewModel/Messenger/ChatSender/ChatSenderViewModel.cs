using System;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using NetChat.Desktop.InnerMessages;
using NetChat.Desktop.ViewModel.Commands;
using NetChat.Desktop.ViewModel.Messenger.ChatArea.Messages;
using NetChat.Services.Messaging.Messages;
using NLog;

namespace NetChat.Desktop.ViewModel.Messenger.ChatSender
{
    public class ChatSenderViewModel : ViewModelBase
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly IMessageSender _messageSender;
        private readonly IMessenger _innerMessageBus;

        public bool HasReply => Reply != null;

        private ReplyObservable _reply;
        public ReplyObservable Reply
        {
            get => _reply;
            set
            {
                if (Set(ref _reply, value))
                    RaisePropertyChanged(nameof(HasReply));
            }
        }

        private string _textMessage;
        public string TextMessage
        {
            get => _textMessage;
            set
            {
                Set(ref _textMessage, value);
                if (((AsyncCommand)SendMessageCommand).Execution != null)
                    SendMessageCommand.Refresh();
            }
        }

#if DEBUG
        public ChatSenderViewModel()
        {
            if (IsInDesignMode)
            {
                TextMessage = "HelloHelloHelloHelloHello\nHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHello";
                Reply = new ReplyObservable("1", "Author N.A.", "Привет или не привет jdcnjkdsc sdcjsdcjk sdchj dschj dschjc sdchj sdch");
            }
            else throw new NotImplementedException("ChatSender without services is not implemented");
        }
#endif

        public ChatSenderViewModel(IMessageSender messageSender,
            IMessenger innerCommunication)
        {
            _messageSender = messageSender ?? throw new ArgumentNullException(nameof(messageSender));
            _innerMessageBus = innerCommunication ?? throw new ArgumentNullException(nameof(innerCommunication));
            _innerMessageBus.Register<ReplyToMessageIM>(this, ReplyToMessageHandler);
        }

        public override void Cleanup()
        {
            _logger.Debug("ViewModel cleaning up: '{0}'", GetType().Name);
            _innerMessageBus.Unregister(this);
            TextMessage = null;
            Reply = null;
            _logger.Debug("ViewModel cleaned up: '{0}'", GetType().Name);
        }

        private void ReplyToMessageHandler(ReplyToMessageIM replyToMessage)
        {
            Reply = new ReplyObservable(replyToMessage.MessageId,
                replyToMessage.AuthorName,
                replyToMessage.Text);
            _logger.Debug("ReplyMessage (id='{0}') pinned to '{1}'",
                Reply.MessageId, GetType().Name);
        }

        private IAsyncCommand _sendMessageCommand;
        public IAsyncCommand SendMessageCommand => _sendMessageCommand
            ?? (_sendMessageCommand = new AsyncCommand(SendMessage, CanSendMessage));
        private async Task SendMessage()
        {
            if (string.IsNullOrEmpty(TextMessage)) return;
            _logger.Debug("Send message button clicked: Text={0}, ReplyMessageId={1}",
                TextMessage != null ? "Not empty" : "Empty",
                Reply != null ? Reply.MessageId : "Empty");
            try
            {
                await _messageSender.SendMessage(
                    new InputMessageTextContent(TextMessage),
                    Reply?.MessageId);
            }
            catch (Exception e)
            {
                _logger.Error("{0} | {1}", e.Message, e.InnerException?.Message);
                _innerMessageBus.Send(new ExceptionIM(e));
                return;
            }
            _logger.Debug("Message sended succusfully!");
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                TextMessage = string.Empty;
                Reply = null;
            });
        }
        private bool CanSendMessage(object obj) => !string.IsNullOrEmpty(TextMessage);


        private RelayCommand _gotoMessageCommand;
        public RelayCommand GoToMessageCommand => _gotoMessageCommand
            ?? (_gotoMessageCommand = new RelayCommand(GoToReplyMessage, HasReply));
        public void GoToReplyMessage()
        {
            if (!HasReply) return;
            _logger.Debug("Go to ReplyMessage (id={0}) clicked", Reply?.MessageId);
            _innerMessageBus.Send(new GoToMessageIM(Reply?.MessageId));
        }

        private RelayCommand _closeRelayMessageCommand;
        public RelayCommand CloseRelayMessageCommand => _closeRelayMessageCommand
            ?? (_closeRelayMessageCommand = new RelayCommand(CloseRelayMessage, HasReply));
        private void CloseRelayMessage()
        {
            _logger.Debug("Close ReplyMessage (id={0}) clicked", Reply?.MessageId);
            Reply = null;
        }
    }
}
