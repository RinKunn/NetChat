using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Autofac;
using Autofac.Extras.CommonServiceLocator;
using CommonServiceLocator;
using NetChat.Desktop.Services.Messaging;
using NetChat.Desktop.Services.Messaging.Messages;
using NetChat.Desktop.Services.Messaging.Users;
using NetChat.FileMessaging;
using NetChat.FileMessaging.Repository;
using NetChat.FileMessaging.Services;

namespace NetChat.Desktop.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        static ViewModelLocator()
        {
            if (!ServiceLocator.IsLocationProviderSet)
            {
                var builder = new ContainerBuilder();
                builder.RegisterType<MainViewModel>();

                var configs = new RepositoriesConfig(Path.Combine(Directory.GetCurrentDirectory(), "chat.dat"), Encoding.UTF8);
                builder.RegisterInstance<RepositoriesConfig>(configs);

                builder.AddFileMessaging(configs);
                builder.RegisterType<UserLoader>().As<IUserLoader>();
                builder.RegisterType<MessageSender>().As<IMessageSender>();
                builder.RegisterType<MessageLoader>().As<IMessageLoader>();
                //builder.RegisterType<ReceiverHub>().As<IReceiverHub>().InstancePerLifetimeScope();

                string username = Environment.UserName;
#if DEBUG
                string currentProcess = Process.GetCurrentProcess().ProcessName;
                int runnedProc = Process.GetProcessesByName(currentProcess).Length;
                Console.WriteLine($"Proces '{currentProcess}' = {runnedProc}");
                if (runnedProc > 1)
                    username += runnedProc.ToString();
#endif
                builder.RegisterInstance<NetChatContext>(new NetChatContext(username));

                builder.Register<IReceiverHub>(c =>
                {
                    var hub = new ReceiverHub(c.Resolve<INetChatHub>(), c.Resolve<IUserLoader>());
                    hub.Connect();
                    return hub;
                }).InstancePerLifetimeScope();
                


                var container = builder.Build();
                ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(container));
            }
        }

        public static void Cleanup()
        {
            var service = ServiceLocator.Current.GetService<IReceiverHub>();
            service.Disconnect();
        }

        public MainViewModel MainViewModel => ServiceLocator.Current.GetInstance<MainViewModel>();
    }

    public static class LocatorServices
    {
        public static TService GetService<TService>(this IServiceLocator locator)
        {
            return (TService)locator.GetService(typeof(TService));
        }
    }
}