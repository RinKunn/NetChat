using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using NetChat.Desktop.Services.Messaging;
using NetChat.Desktop.Services.Messaging.Messages;
using NetChat.Desktop.Services.Messaging.Users;
using NetChat.Desktop.ViewModel.InnerMessages;

namespace NetChat.Desktop.ViewModel.Messenger
{
    public class MessengerViewModel : ViewModelBase
    {
        private readonly IMessenger _innerCommunication;
        private bool _isActivated;
        private ViewModelBase _header;
        private ViewModelBase _chatArea;
        private ViewModelBase _chatSender;
        private ViewModelBase _sideArea;

        public bool IsActivated
        {
            get => _isActivated;
            set => Set(ref _isActivated, value);
        }

        public ViewModelBase Header
        {
            get => _header;
            set
            {
                if (_header != null) _header.Cleanup();
                Set(ref _header, value);
            }
        }

        public ViewModelBase ChatArea
        {
            get => _chatArea;
            set
            {
                if (_chatArea != null) _chatArea.Cleanup();
                Set(ref _chatArea, value);
            }
        }

        public ViewModelBase ChatSender
        {
            get => _chatSender;
            set
            {
                if (_chatSender != null) _chatSender.Cleanup();
                Set(ref _chatSender, value);
            }
        }

        public ViewModelBase SideArea
        {
            get => _sideArea;
            set
            {
                if (_sideArea != null) _sideArea.Cleanup();
                Set(ref _sideArea, value);
            }
        }

#if DEBUG
        public MessengerViewModel()
        {
            if (IsInDesignMode)
            {
                Header = new HeaderViewModel();
                ChatArea = new ChatAreaViewModel();
                ChatSender = new ChatSenderViewModel();
                SideArea = null;
            }
            else throw new NotImplementedException("Messenger without services is not implemented");
        }
#endif

        public MessengerViewModel(
            IUserLoader userLoader, 
            IMessageLoader messageLoader, 
            IMessageSender messageSender,
            IReceiverHub receiverHub, 
            IMessenger innerCommunication,
            UserContext context)
        {
            _innerCommunication = innerCommunication;
            Header = new HeaderViewModel(userLoader, receiverHub);
            ChatArea = new ChatAreaViewModel(messageLoader, receiverHub, innerCommunication);
            ChatSender = new ChatSenderViewModel(context.CurrentUserName, messageSender, innerCommunication);
            SideArea = null;

            _innerCommunication.Register<GoToMessageInnerMessage>(this, (m) => IsActivated = true);
        }

        public override void Cleanup()
        {
            Header?.Cleanup();
            ChatArea?.Cleanup();
            ChatSender?.Cleanup();
            SideArea?.Cleanup();
            _innerCommunication.Unregister<GoToMessageInnerMessage>(this);
            base.Cleanup();
        }
    }
}
