using System;
using System.Security.Permissions;
using System.Threading.Tasks;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Threading;
using NetChat.Desktop.Services.Messaging;
using NetChat.Desktop.Services.Messaging.Users;
using NetChat.Desktop.ViewModel.Commands;
using NetChat.Desktop.ViewModel.InnerMessages;

namespace NetChat.Desktop.ViewModel.Messenger
{
    public class HeaderViewModel : ViewModelBase
    {
        private readonly IUserLoader _userLoader;
        private readonly IReceiverHub _receiverHub;

        private int _participantOnlineCount;

        public string Title { get; }
        public int ParticipantOnlineCount
        {
            get => _participantOnlineCount;
            set => Set(ref _participantOnlineCount, value);
        }

        public HeaderViewModel()
        {
            if (IsInDesignModeStatic)
            {
                Title = "Hello world";
                ParticipantOnlineCount = 189;
            }
            else throw new NotImplementedException();
        }

        public HeaderViewModel(IUserLoader userLoader, IReceiverHub receiverHub, string title = "NetChat")
        {
            Title = title;
            _receiverHub = receiverHub ?? throw new ArgumentNullException(nameof(receiverHub));
            _userLoader = userLoader ?? throw new ArgumentNullException(nameof(userLoader));
            _participantOnlineCount = 0;
            _receiverHub.SubscribeUserStatusChanged(this, (u) => DispatcherHelper.CheckBeginInvokeOnUI(() => ParticipantOnlineCount += u.IsOnline ? 1 : -1));
            
        }

        public override void Cleanup()
        {
            _receiverHub.UnsubscribeUserStatusChanged(this);
            base.Cleanup();
        }

        private IAsyncCommand _loadCommand;
        public IAsyncCommand LoadCommand => _loadCommand ??
            (_loadCommand = new AsyncCommand(LoadUsers));

        public async Task LoadUsers()
        {
            var res = await _userLoader.OnlineUsersCount();
            DispatcherHelper.CheckBeginInvokeOnUI(() => ParticipantOnlineCount = res);
        }
    }

    
}
