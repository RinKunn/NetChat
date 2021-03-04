using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetChat.Desktop.Services.Messaging.Users;
using NetChat.Desktop.ViewModel.Messenger;
using NetChat.FileMessaging.Services.Messages;
using NLog;

namespace NetChat.Desktop.Services.Messaging.Messages
{
    public class MessageLoader : IMessageLoader
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly IMessageService _messageService;
        private readonly IUserLoader _userLoader;

        public MessageLoader(IMessageService messageService, IUserLoader userLoader)
        {
            _messageService = messageService;
            _userLoader = userLoader;
        }

        public async Task<IList<MessageObservable>> LoadMessagesAsync(int limit = 0)
        {
            _logger.Debug("Loading messages...");
            var messages = await _messageService.LoadMessagesAsync(limit);
            if(messages == null)
            {
                _logger.Warn("Loaded message list is empty");
                return null;
            }
            _logger.Debug("Loaded {0} messages", messages.Count);
            return
                await Task.WhenAll(
                    messages
                    .Select(async m =>
                    {
                        var user = await _userLoader.GetUserById(m.SenderId);
                        bool isMe = _userLoader.IsMe(user.UserId);
                        return new TextMessageObservable(m.Text, m.Id, m.DateTime, user, isMe);
                    }));
        }
    }
}
