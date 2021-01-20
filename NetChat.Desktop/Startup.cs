using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Autofac;
using GalaSoft.MvvmLight;
using NetChat.Desktop.Services.Messaging;
using NetChat.Desktop.Services.Messaging.Messages;
using NetChat.Desktop.Services.Messaging.Users;
using NetChat.Desktop.ViewModel;
using NetChat.Desktop.ViewModel.Messenger;
using NetChat.FileMessaging;
using NetChat.FileMessaging.Repository;

namespace NetChat.Desktop
{
    public static class AppContainerBuilder
    {
        private static ContainerBuilder ConfigureServices(ContainerBuilder builder)
        {
            var configs = new RepositoriesConfig(Path.Combine(Directory.GetCurrentDirectory(), "chat.dat"), Encoding.UTF8);
            builder.RegisterInstance<RepositoriesConfig>(configs);

            builder.RegisterAppContext();
            builder.AddFileMessaging(configs);
            builder.RegisterAppServices();
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
            builder.RegisterType<HeaderViewModel>().Named<ViewModelBase>(typeof(HeaderViewModel).ToString());
            builder.RegisterType<ChatAreaViewModel>().Named<ViewModelBase>(typeof(ChatAreaViewModel).ToString());
            builder.RegisterType<ChatSenderViewModel>().Named<ViewModelBase>(typeof(ChatSenderViewModel).ToString());
            return builder;
        }

        public static ContainerBuilder RegisterAppServices(this ContainerBuilder builder)
        {
            builder.RegisterType<UserLoader>().As<IUserLoader>();
            builder.RegisterType<MessageSender>().As<IMessageSender>();
            builder.RegisterType<MessageLoader>().As<IMessageLoader>();
            builder.RegisterType<ReceiverHub>().As<IReceiverHub>().InstancePerLifetimeScope();
            return builder;
        }

        public static ContainerBuilder RegisterAppContext(this ContainerBuilder builder)
        {
            string username = Environment.UserName;
#if DEBUG
            string currentProcess = Process.GetCurrentProcess().ProcessName;
            int runnedProc = Process.GetProcessesByName(currentProcess).Length;
            if (runnedProc > 1)
                username += runnedProc.ToString();
#endif
            builder.RegisterInstance<NetChatContext>(new NetChatContext(username));
            return builder;
        }
    }
}
