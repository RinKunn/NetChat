using Autofac;
using NetChat.Services.Authentication;
using NetChat.Services.Messaging.Chats;
using NetChat.Services.Messaging.Messages;
using NetChat.Services.Messaging.Users;
using NetChat.Services.Mock;

namespace NetChat.Services
{
    public static class DependencyInjections
    {
        public static ContainerBuilder RegisterAppServices(this ContainerBuilder builder)
        {
            return builder;
        }

        public static ContainerBuilder RegisterMockAppServices(this ContainerBuilder builder)
        {
            builder.RegisterType<MockAuthenticator>().As<IAuthenticator>();
            builder.RegisterType<MockChatLoader>().As<IChatLoader>();
            builder.RegisterType<MockMessageLoader>().As<IMessageLoader>();
            builder.RegisterType<MockMessageSender>().As<IMessageSender>();
            builder.RegisterType<MockMessageUpdater>().As<IMessageUpdater>().SingleInstance();
            builder.RegisterType<MockUserLoader>().As<IUserLoader>();
            builder.RegisterType<MockUserUpdater>().As<IUserUpdater>().SingleInstance();
            return builder;
        }
    }
}
