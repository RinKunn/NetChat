using System.Threading.Tasks;

namespace NetChat.Services.Messaging.Messages
{
    public interface IMessageSender
    {
        Task<bool> SendMessage(InputMessageContent content, string replyToMessageId);
    }
}
