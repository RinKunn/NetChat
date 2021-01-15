using System;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Threading;
using NetChat.Desktop.Services.Messaging.Messages;
using NetChat.Desktop.ViewModel.Commands;
using NetChat.Desktop.ViewModel.InnerMessages;

namespace NetChat.Desktop.ViewModel.Messenger
{
    public class ChatSenderViewModel :ViewModelBase
    {
        private string _userId;
        private IMessageSender _messageSender;

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

        public ChatSenderViewModel()
        {
            if(IsInDesignModeStatic)
            {
                TextMessage = "HelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHello";
            }
            else throw new NotImplementedException();
        }

        public ChatSenderViewModel(string userId, IMessageSender messageSender)
        {
            _messageSender = messageSender ?? throw new ArgumentNullException(nameof(messageSender));
            _userId = userId ?? throw new ArgumentNullException(nameof(userId));
        }


        private IAsyncCommand _sendMessageCommand;
        public IAsyncCommand SendMessageCommand => _sendMessageCommand ??
            (_sendMessageCommand = new AsyncCommand(SendMessage, CanSendMessage));

        private async Task SendMessage()
        {
            try
            {
                await _messageSender.SendMessage(new InputMessageText(TextMessage, _userId));
                DispatcherHelper.CheckBeginInvokeOnUI(() => TextMessage = string.Empty);
            }
            catch(Exception e)
            {
                MessengerInstance.Send<ExceptionIMessage>(new ExceptionIMessage(e.Message));
                throw e;
            }
        }
        private bool CanSendMessage(object obj) => !string.IsNullOrWhiteSpace(TextMessage);
    }
}
