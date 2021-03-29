using System;
using Autofac;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using NetChat.Desktop.ViewModel;
using NetChat.Desktop.ViewModel.Factories;
using NetChat.Desktop.ViewModel.Messenger;
using NetChat.Desktop.ViewModel.Messenger.ChatArea;
using NetChat.Desktop.ViewModel.Messenger.ChatArea.Factories;
using NetChat.Desktop.ViewModel.Messenger.ChatHeader;
using NetChat.Desktop.ViewModel.Messenger.ChatSender;
using NetChat.Desktop.ViewModel.Notifications;
using NetChat.Services;

namespace NetChat.Desktop
{
    public static class AppContainerBuilder
    {
        private static ContainerBuilder ConfigureServices(ContainerBuilder builder)
        {
            //string filename = ConfigurationManager.AppSettings.Get("SourcePath");
            //if (filename == null)
            //    throw new ArgumentNullException(nameof(filename));
             
            builder.RegisterAppContext();
            builder.RegisterType<ViewModelFactory>().As<IViewModelFactory>();
            builder.RegisterType<MessageFactory>().As<IMessageFactory>();
            builder.RegisterType<Messenger>().As<IMessenger>();
            //builder.RegisterAppServices();
            builder.RegisterMockAppServices();
            builder.RegisterViewModels();

            return builder;
        }

        public static IContainer CreateServicesContainer()
        {
            var builder = new ContainerBuilder();
            builder = ConfigureServices(builder);
            return builder.Build();
        }
    }


    public static class ContainerHelper
    {
        public static TViewModel ResolveViewModel<TViewModel>(this ILifetimeScope container) where TViewModel : ViewModelBase
        {
            return (TViewModel)container.ResolveNamed<ViewModelBase>(typeof(TViewModel).ToString());
        }
    }

    public static class Registrations
    {
        public static ContainerBuilder RegisterViewModels(this ContainerBuilder builder)
        {
            builder.RegisterType<MainViewModel>().Named<ViewModelBase>(typeof(MainViewModel).ToString());
            builder.RegisterType<ChatHeaderViewModel>().Named<ViewModelBase>(typeof(ChatHeaderViewModel).ToString());
            builder.RegisterType<ChatAreaViewModel>().Named<ViewModelBase>(typeof(ChatAreaViewModel).ToString());
            builder.RegisterType<ChatSenderViewModel>().Named<ViewModelBase>(typeof(ChatSenderViewModel).ToString());
            builder.RegisterType<NotificationsViewModel>().Named<ViewModelBase>(typeof(NotificationsViewModel).ToString());
            builder.RegisterType<MessengerViewModel>().Named<ViewModelBase>(typeof(MessengerViewModel).ToString());
            return builder;
        }

        public static ContainerBuilder RegisterAppContext(this ContainerBuilder builder)
        {
            var notifyConfig = new NotificationConfiguration()
            {
                EnableParticipantNotifications = false,
                EnableMessageNotifications = true,
                HideTimeout = TimeSpan.FromSeconds(5),
                ShowingMaxCount = 3
            };
            builder.RegisterInstance<NotificationConfiguration>(notifyConfig);
            return builder;
        }
    }
}
