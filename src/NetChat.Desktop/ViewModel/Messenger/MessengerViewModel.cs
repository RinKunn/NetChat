using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using NetChat.Desktop.InnerMessages;
using NetChat.Desktop.ViewModel.Messenger.ChatArea;
using NetChat.Desktop.ViewModel.Messenger.ChatArea.Factories;
using NetChat.Desktop.ViewModel.Messenger.ChatHeader;
using NetChat.Desktop.ViewModel.Messenger.ChatSender;
using NetChat.Services.Messaging;
using NetChat.Services.Messaging.Chats;
using NetChat.Services.Messaging.Messages;
using NetChat.Services.Messaging.Users;

namespace NetChat.Desktop.ViewModel.Messenger
{
    public class MessengerViewModel : ViewModelBase
    {
        private readonly IMessenger _innerMessageBus;
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
                Header = new ChatHeaderViewModel();
                ChatArea = new ChatAreaViewModel();
                ChatSender = new ChatSenderViewModel();
                SideArea = null;
            }
            else throw new NotImplementedException("Messenger without services is not implemented");
        }
#endif

        public MessengerViewModel(
            IUserLoader userLoader, 
            IUserUpdater userUpdater,
            IMessageLoader messageLoader, 
            IMessageSender messageSender,
            IMessageUpdater messageUpdater,
            IMessageFactory messageFactory,
            IChatLoader chatLoader, 
            IMessenger innerCommunication)
        {
            _innerMessageBus = innerCommunication;
            Header = new ChatHeaderViewModel(
                userLoader, userUpdater);
            ChatArea = new ChatAreaViewModel(
                chatLoader, messageLoader, 
                messageUpdater, innerCommunication, messageFactory);
            ChatSender = new ChatSenderViewModel(
                messageSender, innerCommunication);
            SideArea = null;

            _innerMessageBus.Register<GoToMessageIM>(this, (m) => IsActivated = true);
        }

        public override void Cleanup()
        {
            Header?.Cleanup();
            ChatArea?.Cleanup();
            ChatSender?.Cleanup();
            SideArea?.Cleanup();
            _innerMessageBus.Unregister<GoToMessageIM>(this);
            base.Cleanup();
        }
    }
}
