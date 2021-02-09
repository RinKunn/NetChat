using System;
using GalaSoft.MvvmLight;
using NetChat.Desktop.Services.Messaging;
using NetChat.Desktop.Services.Messaging.Messages;
using NetChat.Desktop.Services.Messaging.Users;
using NetChat.Desktop.ViewModel.InnerMessages;
using NetChat.Desktop.ViewModel.Messenger;

namespace NetChat.Desktop.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private bool _isVisible;
        private ViewModelBase _header;
        private ViewModelBase _chatArea;
        private ViewModelBase _chatSender;
        private ViewModelBase _sideArea;


        public bool IsVisible
        {
            get => _isVisible;
            set => Set(ref _isVisible, value);
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



        public MainViewModel(IUserLoader userLoader, IMessageLoader messageLoader, IMessageSender messageSender, IReceiverHub receiverHub, UserContext context)
        {
            if(IsInDesignModeStatic)
            {
                Header = new HeaderViewModel();
                ChatArea = new ChatAreaViewModel();
                ChatSender = new ChatSenderViewModel();
                SideArea = null;
            }
            else
            {
                Header = new HeaderViewModel(userLoader, receiverHub);
                ChatArea = new ChatAreaViewModel(messageLoader, receiverHub);
                ChatSender = new ChatSenderViewModel(context.CurrentUserName, messageSender);
                SideArea = null;
            }
            MessengerInstance.Register<ExceptionIMessage>(this, (ex) => Console.WriteLine("Error occured: " + ex.ErrorMessage));
            MessengerInstance.Register<GoToMessageIMessage>(this, (m) =>
            {
                if (!IsVisible) IsVisible = true;
            });
        }
    }
}