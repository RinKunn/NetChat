using System.Linq;
using NetChat.Desktop.ViewModel.Messenger.ChatArea.Messages;
using NetChat.Services.Messaging.Messages;

namespace NetChat.Desktop.ViewModel.Messenger.ChatArea.Factories
{
    public class MessageFactory : IMessageFactory
    {
        public MessageObservable ToObservable(Message message)
        {
            switch(message.MessageData.Content)
            {
                case MessageTextContent textContent:
                    return GetTextMessageObservable(message, textContent.Text);
                default:
                    return GetUnsupportedMessageObservable(message);
            }
        }

        private TextMessageObservable GetTextMessageObservable(Message message, string text)
        {
            ReplyObservable reply = GetReplyObservable(message);

            return new TextMessageObservable(
                message.MessageData.Id,
                message.MessageData.Date,
                message.UserData.UserName,
                message.MessageData.IsOutgoing,
                text,
                reply);
        }

        private UnsupportedMessageObservable GetUnsupportedMessageObservable(
            Message message)
        {
            ReplyObservable reply = GetReplyObservable(message);

            return new UnsupportedMessageObservable(
                message.MessageData.Id,
                message.MessageData.Date,
                message.UserData.UserName,
                message.MessageData.IsOutgoing,
                reply);
        }

        private ReplyObservable GetReplyObservable(Message message)
        {
            if (message.ReplyToMessage == null)
                return null;

            return new ReplyObservable(
                message.ReplyToMessage.MessageData.Id,
                GetReplyAuthor(message),
                GetReplyText(message));
        }

        private string GetReplyAuthor(Message message)
        {
            return message.ReplyToMessage.UserData.UserName;
        }

        private string GetReplyText(Message message)
        {
            if (message == null) return null;
            string text = null;
            switch (message.ReplyToMessage.MessageData.Content)
            {
                case MessageTextContent textContent:
                    text = textContent.Text;
                    break;
                default:
                    text = "*Unsupported content*";
                    break;
            }
            return new string(
               text.Take(64)
                   .TakeWhile(c => c != '\n' && c != '\r')
                   .ToArray());
        }
    }
}
