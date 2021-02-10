using System;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using NetChat.Desktop.Services.Messaging.Messages;
using NetChat.Desktop.ViewModel.Commands;
using NetChat.Desktop.ViewModel.InnerMessages;

namespace NetChat.Desktop.ViewModel.Messenger
{
    public class ChatSenderViewModel :ViewModelBase
    {
        private readonly string _currentUserId;
        private readonly IMessageSender _messageSender;
        private readonly IMessenger _innerCommunication;

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
            if(IsInDesignMode)
            {
                TextMessage = "HelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHello";
            }
            else throw new NotImplementedException("ChatSender without services is not implemented");
        }
#endif

        public ChatSenderViewModel(string userId, IMessageSender messageSender, IMessenger innerCommunication)
        {
            _messageSender = messageSender ?? throw new ArgumentNullException(nameof(messageSender));
            _currentUserId = userId ?? throw new ArgumentNullException(nameof(userId));
            _innerCommunication = innerCommunication ?? throw new ArgumentNullException(nameof(innerCommunication));
        }


        private IAsyncCommand _sendMessageCommand;
        public IAsyncCommand SendMessageCommand => _sendMessageCommand ??
            (_sendMessageCommand = new AsyncCommand(SendMessage, CanSendMessage));

        private async Task SendMessage()
        {
            if (string.IsNullOrEmpty(TextMessage)) return;
            try
            {
                await _messageSender.SendMessage(new SendingTextMessage(_currentUserId, TextMessage));
                DispatcherHelper.CheckBeginInvokeOnUI(() => TextMessage = string.Empty);
            }
            catch(Exception e)
            {
                _innerCommunication.Send(new ExceptionInnerMessage(e.Message));
                throw e;
            }
        }
        private bool CanSendMessage(object obj) => !string.IsNullOrEmpty(TextMessage);
    }
}
