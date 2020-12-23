using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetChat.Desktop.Services.Messaging.Users;

namespace NetChat.Desktop.Services.Messaging.Messages
{
    public interface IMessageLoader
    {
        Task<IEnumerable<Message>> LoadMessagesAsync(int limit = 0);
    }

    public class DefaultMessageLoader : IMessageLoader
    {
        // userService to get user info
        //

        public async Task<IEnumerable<Message>> LoadMessagesAsync(int limit = 0)
        {
            await Task.Delay(2000).ConfigureAwait(false);
            var messages = new List<Message>();
            messages.Add(new MessageText() { Id = "1", DateTime = DateTime.Now, Text = "User1HelloOffline", Sender = new User() { Id = "User1", Status = UserStatus.Offline, StatusChangedDateTime = DateTime.Now }, IsOriginNative = false });
            messages.Add(new MessageText() { Id = "2", DateTime = DateTime.Now, Text = "User2HelloOnlineMe", Sender = new User() { Id = "User2", Status = UserStatus.Online, StatusChangedDateTime = DateTime.Now }, IsOriginNative = true });
            messages.Add(new MessageText() { Id = "3", DateTime = DateTime.Now, Text = "User3HelloOnline", Sender = new User() { Id = "User3", Status = UserStatus.Online, StatusChangedDateTime = DateTime.Now }, IsOriginNative = false });
            return messages;
        }
    }
}
