using System.Windows;
using GalaSoft.MvvmLight.Threading;
using NetChat.Desktop.ViewModel;
using NetChat.FileMessaging.Services.Users;
using Locator = CommonServiceLocator.ServiceLocator;

namespace NetChat.Desktop
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IUserService _userService;

        public App() : base()
        {
            new ViewModelLocator();
            _userService = Locator.Current.GetService<IUserService>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            DispatcherHelper.Initialize();
            _userService.Logon(Locator.Current.GetService<NetChatContext>().CurrentUserName);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            ViewModelLocator.Cleanup();
            _userService.Logout(Locator.Current.GetService<NetChatContext>().CurrentUserName);
            DispatcherHelper.Reset();
            base.OnExit(e);
        }
    }
}
