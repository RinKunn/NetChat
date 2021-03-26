using System;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Threading;
using NetChat.Desktop.Commands;
using NetChat.Services.Messaging.Users;
using NLog;

namespace NetChat.Desktop.ViewModel.Messenger.ChatHeader
{
    public class ChatHeaderViewModel : ViewModelBase
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly IUserLoader _userLoader;
        private readonly IUserUpdater _usersUpdater;

        private int _usersOnlineCount;

        public string Title { get; }
        public int UsersOnlineCount
        {
            get => _usersOnlineCount;
            set => Set(ref _usersOnlineCount, value);
        }

#if DEBUG
        public ChatHeaderViewModel()
        {
            if (IsInDesignMode)
            {
                Title = "Hello world";
                UsersOnlineCount = 189;
                ((AsyncCommand)LoadUsersCountCommand).Execution = new NotifyTaskCompletion(Task.CompletedTask);
            }
            else throw new NotImplementedException("Header without services is not implemented");
        }
#endif

        public ChatHeaderViewModel(IUserLoader userLoader,
            IUserUpdater usersUpdater,
            string title = "NetChat")
        {
            Title = title;
            _usersUpdater = usersUpdater ?? throw new ArgumentNullException(nameof(_usersUpdater));
            _userLoader = userLoader ?? throw new ArgumentNullException(nameof(userLoader));
            _usersOnlineCount = 0;
            _usersUpdater.Register(this, OnUserStatusChanged);
        }

        private void OnUserStatusChanged(UserStatusData userStatus)
        {
            DispatcherHelper.CheckBeginInvokeOnUI(()
                => UsersOnlineCount += userStatus.IsOnline ? 1 : -1);
        }

        public override void Cleanup()
        {
            _logger.Debug("ViewModel cleaning up: '{0}'", GetType().Name);
            _usersUpdater.Unregister(this);
            UsersOnlineCount = 0;
            base.Cleanup();
            _logger.Debug("ViewModel cleaned up: '{0}'", GetType().Name);
        }

        private IAsyncCommand _loadUsersCountCommand;
        public IAsyncCommand LoadUsersCountCommand => _loadUsersCountCommand
            ?? (_loadUsersCountCommand = new AsyncCommand(LoadUsersCountAsync));

        public async Task LoadUsersCountAsync()
        {
            _logger.Debug("Loading online users count...");
            var res = await _userLoader.GetOnlineUsersCount();
            DispatcherHelper.CheckBeginInvokeOnUI(() => UsersOnlineCount = res);
            _logger.Debug("Online users count = {0}", res);
        }
    }
}
