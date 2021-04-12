using System;
using System.Threading.Tasks;
using NetChat.Services.FileMessaging;
using NetChat.Services.Messaging.Users;
using NetChat.Services.Authentication.Exceptions;
using NetChat.Services.Messaging.Messages.Storage;

namespace NetChat.Services.Authentication
{
    public class Authenticator : IAuthenticator
    {
        private readonly IMessageDataRepository _messageDataRepository;
        private readonly IUserLoader _userLoader;

        public Authenticator(IMessageDataRepository messageDataRepository, IUserLoader userLoader)
        {
            _messageDataRepository = messageDataRepository 
                ?? throw new ArgumentNullException(nameof(messageDataRepository));
            _userLoader = userLoader 
                ?? throw new ArgumentNullException(nameof(userLoader));
        }

        public async Task Logon()
        {
            var me = _userLoader.GetMe();
            await _messageDataRepository.AddAsync(
                new MessageDataEntity(me.Id, "Logon"));
        }

        public async Task Logout()
        {
            var me = _userLoader.GetMe();
            await _messageDataRepository.AddAsync(
                new MessageDataEntity(me.Id, "Logout"));
        }
    }
}
