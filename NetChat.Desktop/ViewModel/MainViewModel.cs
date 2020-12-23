using System;
using GalaSoft.MvvmLight;
using NetChat.Desktop.ViewModel.InnerMessages;
using NetChat.Desktop.ViewModel.Messenger;

namespace NetChat.Desktop.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private ViewModelBase _header;
        private ViewModelBase _chatArea;
        private ViewModelBase _chatSender;
        private ViewModelBase _sideArea;

        public ViewModelBase Header
        {
            get => _header;
            set => Set(ref _header, value);
        }

        public ViewModelBase ChatArea
        {
            get => _chatArea;
            set => Set(ref _chatArea, value);
        }

        public ViewModelBase ChatSender
        {
            get => _chatSender;
            set => Set(ref _chatSender, value);
        }

        public ViewModelBase SideArea
        {
            get => _sideArea;
            set => Set(ref _sideArea, value);
        }

        public MainViewModel()
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
                Header = new HeaderViewModel("Default chat");
                ChatArea = new ChatAreaViewModel(null);
                ChatSender = new ChatSenderViewModel("User");
                SideArea = null;
            }
            MessengerInstance.Register<ExceptionIMessage>(this, (ex) => Console.WriteLine("Error occured: " + ex.ErrorMessage));
        }
    }
}