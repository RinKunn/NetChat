using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NetChat.FileMessaging.Models;

namespace NetChat.FileMessaging.Services.Messages
{
    public interface IFileMessageService
    {
        Task<IList<MessageData>> LoadMessagesAsync(int limit = 0, CancellationToken token = default);
        Task<MessageData> SendMessage(InputMessageData inputMessageData);
        Task<MessageData> GetMessageById(string messageId);
        //TODO: implement data virtualisation: nextpage, prevpage
    }
}
