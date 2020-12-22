using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using NetChat.Desktop.Services.Messaging.Messages;

namespace NetChat.Desktop.ViewModel.Messenger
{
    public class ChatSenderViewModel :ViewModelBase
    {
        private IMessageSender _messageSender;

        private string _textMessage;
        public string TextMessage
        {
            get => _textMessage;
            set => Set(ref _textMessage, value);
        }

        public ChatSenderViewModel()
        {
            if (IsInDesignModeStatic)
            {
                TextMessage = "HELLWORLDHELLWORLDHELLWORLDHELLWORLDHELLWORLDHELLWORLDHELLWORLDHELLWORLDHELLWORLDHELLWORLD";
                //TextMessage = string.Empty;
            }
        }

        public ChatSenderViewModel(IMessageSender messageSender)
        {
            _messageSender = messageSender;
        }

        private ICommand _sendMessageCommand;
        public ICommand SendMessageCommand => _sendMessageCommand ??
            (_sendMessageCommand = new RelayCommand(SendMessage, CanSendMessage));

        private void SendMessage()
        {

        }
        private bool CanSendMessage() => !string.IsNullOrWhiteSpace(TextMessage);
    }
}
