using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetChat.Desktop.Repository;
using NetChat.Desktop.Services.Messaging.Users;

namespace NetChat.Desktop.Services.Messaging.Messages
{
    public delegate void OnMessageReceivedHandler(Message message);

    public interface IMessageUpdater
    {
        event OnMessageReceivedHandler OnMessageReceived;
    }


    public class MessageUpdater : IMessageUpdater
    {
        public event OnMessageReceivedHandler OnMessageReceived;
        private readonly IMessageReceiver _messageReceiver;
        private readonly IUserLoader _userLoader;

        public MessageUpdater(IMessageReceiver messageReceiver, IUserLoader userLoader)
        {
            _messageReceiver = messageReceiver;
            _userLoader = userLoader;
            _messageReceiver.OnMessageReceived += _messageReceiver_OnMessageReceived;
        }

        private void _messageReceiver_OnMessageReceived(MessageData message)
        {
            OnMessageReceived?.BeginInvoke(new MessageText()
            {
                Id = message.Id,
                DateTime = message.DateTime,
                IsOriginNative = _userLoader.IsMe(message.UserId),
                Text = ((MessageTextData)message).Text,
                Sender = _userLoader.GetUserById(message.UserId).Result
            }, );
        }
    }
}
