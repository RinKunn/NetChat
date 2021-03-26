using System;
using GalaSoft.MvvmLight;
using NetChat.Desktop.ViewModel.Factories;
using NetChat.Desktop.ViewModel.Messenger;
using NetChat.Desktop.ViewModel.Notifications;

namespace NetChat.Desktop.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IViewModelFactory _viewModelFactory;
        private ViewModelBase _messenger;
        private ViewModelBase _notificator;


        public ViewModelBase Messenger
        {
            get => _messenger;
            set
            {
                if (_messenger != null) _messenger.Cleanup();
                Set(ref _messenger, value);
            }
        }

        public ViewModelBase Notificator
        {
            get => _notificator;
            set
            {
                if (_notificator != null) _notificator.Cleanup();
                Set(ref _notificator, value);
            }
        }

#if DEBUG
        internal MainViewModel()
        {
            if(IsInDesignMode)
            {
                Messenger = new MessengerViewModel();
                Notificator = new NotificationsViewModel();
            }
            else throw new NotImplementedException("MainViewModel without services is not implemeted");
        }
#endif

        public MainViewModel(IViewModelFactory viewModelFactory)
        {
            _viewModelFactory = viewModelFactory ?? throw new ArgumentNullException(nameof(viewModelFactory));
            Messenger = _viewModelFactory.CreateViewModel<MessengerViewModel>() 
                ?? throw new ArgumentNullException($"Cannot create 'MessengerViewModel'");
            Notificator = _viewModelFactory.CreateViewModel<NotificationsViewModel>() 
                ?? throw new ArgumentNullException($"Cannot create 'NotificationsViewModel'");
        }
    }
}