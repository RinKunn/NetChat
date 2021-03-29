using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetChat.Services.Authentication;
using NetChat.Services.Messaging.Chats;
using NetChat.Services.Messaging.Messages;
using NetChat.Services.Messaging.Users;

namespace NetChat.Services.Mock
{
    public class MockAuthenticator : IAuthenticator
    {
        public Task Logon()
        {
            return Task.CompletedTask;
        }

        public Task Logout()
        {
            return Task.CompletedTask;
        }
    }

    public class MockChatLoader : IChatLoader
    {
        public ChatData GetChatData()
        {
            return new ChatData(null);
        }

        public void SetChatData(ChatData chatData)
        {
            return;
        }
    }

    public class MockMessageLoader : IMessageLoader
    {
        public async Task<IList<Message>> LoadMessagesAsync(string fromMessId, int limit = 0)
        {
            await Task.Delay(2000);
            IList<Message> res = new List<Message>();
            res.Add(new Message(
                    new MessageData("1", "User1", DateTime.Now, false, new MessageTextContent("Hello world"), null),
                    new UserData("User1", "User A.One"),
                    null));
            res.Add(new Message(
                    new MessageData("2", "User2", DateTime.Now, false, new MessageTextContent("Hello world hello User1"), "1"),
                    new UserData("User2", "User B.Two"),
                    res[0]));
            res.Add(new Message(
                    new MessageData("3", "User3", DateTime.Now, true, new MessageTextContent("Hello world hello User1, User2"), "2"),
                    new UserData("User3", "It is me"),
                    res[1]));
            res.Add(new Message(
                    new MessageData("4", "User1", DateTime.Now, false, new MessageTextContent("Hello!!"), null),
                    new UserData("User1", "User A.One"),
                    null));
            return res;
        }
    }

    public class MockMessageSender : IMessageSender
    {
        private readonly IMessageUpdater _messageUpdater;

        public MockMessageSender(IMessageUpdater messageUpdater)
        {
            _messageUpdater = messageUpdater;
        }

        public Task<bool> SendMessage(InputMessageContent content, string replyToMessageId)
        {
            var upd = (MockMessageUpdater)_messageUpdater;
            if(content is InputMessageTextContent textContent)
            upd.Run(new Message(
                    new MessageData(Guid.NewGuid().ToString(), "User3", DateTime.Now, true, new MessageTextContent(textContent.Text), replyToMessageId),
                    new UserData("User3", "It is me"),
                    null));
            return Task.FromResult(true);
        }
    }

    public class MockMessageUpdater : IMessageUpdater
    {
        private readonly ConcurrentDictionary<object, WeakReference<Action<Message>>> _dict =
            new ConcurrentDictionary<object, WeakReference<Action<Message>>>();

        public void Run(Message message)
        {
            foreach(var kv in _dict)
            {
                if (kv.Value.TryGetTarget(out var action))
                    action.Invoke(message);
            }
        }

        public void Register(object obj, Action<Message> action)
        {
            if(!_dict.ContainsKey(obj))
            {
                _dict.TryAdd(obj, new WeakReference<Action<Message>>(action));
            }
        }

        public void Unregister(object obj)
        {
            if (_dict.ContainsKey(obj))
            {
                _dict.TryRemove(obj, out var _);
            }
        }
    }

    public class MockUserLoader : IUserLoader
    {
        public Task<int> GetOnlineUsersCount()
        {
            return Task.FromResult(3);
        }
    }

    public class MockUserUpdater : IUserUpdater
    {
        public void Register(object obj, Action<UserStatusData> action)
        {
            return;
        }

        public void Unregister(object obj)
        {
            return;
        }
    }
}
