using System.Windows;
using Autofac;
using GalaSoft.MvvmLight.Threading;
using NetChat.Services.Messaging;
using NetChat.Services;
using NetChat.Desktop.View.Messenger;
using NetChat.Desktop.View.Notificator;
using NetChat.Desktop.ViewModel;
using NLog;
using NetChat.Services.Authentication;

namespace NetChat.Desktop
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private IAuthenticator _authenticator;
        private IContainer _container;
        
        protected override void OnStartup(StartupEventArgs e)
        {
            _logger.Info(new string('-', 20));
            _logger.Info("App initing...");
            _container = AppContainerBuilder.CreateServicesContainer();
            _logger.Info("IOC is builded");

            _authenticator = _container.Resolve<IAuthenticator>();
            _authenticator.Logon().Wait(1000);
            _logger.Info("Current user logged in");

            _container.StartServices();
            _logger.Info("Connected to Hub");

            base.OnStartup(e);
            DispatcherHelper.Initialize();
            _logger.Info("DispatcherHelper initialized");
            _logger.Info("App inited");
            _logger.Info(new string('-', 20));
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _container.StopServices();
            _authenticator.Logout().Wait(1000);
            _logger.Info("Current user logged out");

            DispatcherHelper.Reset();
            base.OnExit(e);
            _logger.Info("App closed");
            LogManager.Shutdown();
        }


        private void Application_Startup(object sender, StartupEventArgs e)
        {
            this.MainWindow = new MessengerView();
            var notifier = new NotificationsArea();
            this.MainWindow.Closing += (o, ee) => notifier.Close();

            var viewModel = _container.ResolveViewModel<MainViewModel>();
            this.MainWindow.DataContext = viewModel.Messenger;
            notifier.DataContext = viewModel.Notificator;
            this.MainWindow.Show();
            notifier.Show();
        }
    }
}
