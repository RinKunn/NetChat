﻿using System.Threading.Tasks;
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
            _logger.Info("App initing...");
            _container = AppContainerBuilder.CreateServicesContainer();
            _logger.Info("IOC is builded");

            _userService = _container.Resolve<IUserService>();
            _username = _container.Resolve<NetChatContext>().CurrentUserName;

            Task.WaitAll(new Task[] { _userService.Logon(_username) }, 1000);
            _logger.Info("Current user '{0}' logged in", _username);

            ConnectToAllHub();
            _logger.Info("Connected to Hub");

            base.OnStartup(e);
            DispatcherHelper.Initialize();
            _logger.Info("DispatcherHelper initialized");
            _logger.Info("App inited");
        }

        protected override void OnExit(ExitEventArgs e)
        {
            DisconectFromAllHubs();
            Task.WaitAll(new Task[] { _userService.Logout(_username) }, 1000);
            _logger.Info("Current user '{0}' logged out", _username);

            DispatcherHelper.Reset();
            base.OnExit(e);
            _logger.Info("App closed");
            LogManager.Shutdown();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            this.MainWindow = new MainWindow();
            this.MainWindow.DataContext = _container.ResolveViewModel<MainViewModel>();
            this.MainWindow.Show();
        }

        private void ConnectToAllHub()
        {
            var hub = _container.Resolve<IReceiverHub>();
            hub.Connect();
        }

        private void DisconectFromAllHubs()
        {
            var hub = _container.Resolve<IReceiverHub>();
            hub.Disconnect();
        }
    }
}
