using Autofac;
using NetChat.FileMessaging.Repository;
using NetChat.FileMessaging.Repository.Messages;
using NetChat.FileMessaging.Repository.UserStatuses;
using NetChat.FileMessaging.Services;
using NetChat.FileMessaging.Services.Messages;
using NetChat.FileMessaging.Services.Users;

namespace NetChat.FileMessaging
{
    public static class AutofacDependencyInjection
    {
        public static ContainerBuilder AddFileMessaging(this ContainerBuilder container, RepositoriesConfig config)
        {
            container.RegisterInstance(config);
            container.RegisterType<TextMessageDataFileRepository>().As<ITextMessageDataRepository>();
            container.RegisterType<UserStatusFileRepository>().As<IUserStatusRepository>();

            
            container.RegisterType<DefaultMessageService>().As<IMessageService>();
            container.RegisterType<DefaultUserService>().As<IUserService>();

            var hub = new FileNetChatHub(config);
            container.RegisterInstance(hub).As<INetChatHub>().OwnedByLifetimeScope();

            return container;
        }
    }
}
