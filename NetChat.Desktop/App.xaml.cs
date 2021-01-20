using System.Threading.Tasks;
using System.Windows;
using Autofac;
using GalaSoft.MvvmLight.Threading;
using NetChat.Desktop.Services.Messaging;
using NetChat.Desktop.View.Messenger;
using NetChat.Desktop.ViewModel;
using NetChat.FileMessaging.Services.Users;
using NLog;

namespace NetChat.Desktop
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private IUserService _userService;
        private IContainer _container;
        private string _username;
        
        protected override void OnStartup(StartupEventArgs e)
        {
            _logger.Info(new string('-', 20));
            _container = AppContainerBuilder.CreateServicesContainer();
            
            _userService = _container.Resolve<IUserService>();
            _username = _container.Resolve<NetChatContext>().CurrentUserName;

            Task.WaitAll(new Task[] { _userService.Logon(_username) }, 1000);
            ConnecToAllHub();

            base.OnStartup(e);
            DispatcherHelper.Initialize();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            DisconectFromAllHubs();
            Task.WaitAll(new Task[] { _userService.Logout(_username) }, 1000);

            DispatcherHelper.Reset();
            base.OnExit(e);
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            _logger.Debug("Application_Startup");
            this.MainWindow = new MainWindow();
            this.MainWindow.DataContext = _container.ResolveViewModel<MainViewModel>();
            this.MainWindow.Show();
        }

        private void ConnecToAllHub()
        {
            var hub = _container.Resolve<IReceiverHub>();
            hub.Connect();
            _logger.Debug("ConnecToAllHub: Logeer {0} : {1}", hub.IsConnected, hub.GetHashCode());

        }


        private void DisconectFromAllHubs()
        {
            var hub = _container.Resolve<IReceiverHub>();
            hub.Disconnect();
            _logger.Debug("DisconectFromAllHubs: Logeer {0} : {1}", hub.IsConnected, hub.GetHashCode());
        }
    }
}
