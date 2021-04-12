using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading;
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

        public Task SetupParameters()
        {
            throw new NotImplementedException();
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
        private MemoryCache _cache = MemoryCache.Default;

        public MockMessageLoader()
        {
            GenerateMessageData();
            GenerateUserData();
        }

        public void GenerateMessageData()
        {
            CacheItemPolicy policy = new CacheItemPolicy();
            _cache.Set("m1", new MessageData("m1", "User1", DateTime.Now.AddDays(-1), false, new MessageTextContent("Hello world"), null), null);
            _cache.Set("m2", new MessageData("m2", "User2", DateTime.Now.AddDays(-1), false, new MessageTextContent("Hello world hello User1"), "m1"), null);
            _cache.Set("m3", new MessageData("m3", "User3", DateTime.Now.AddDays(-1), true, new MessageTextContent("Hello world hello User1, User2"), "m2"), null);
            _cache.Set("m4", new MessageData("m4", "User1", DateTime.Now, false, new MessageTextContent("Hello!!"), null), null);
        }

        public void GenerateUserData()
        {
            CacheItemPolicy policy = new CacheItemPolicy();
            _cache.Set("User1", new UserData("User1", "User A.One"), policy);
            _cache.Set("User2", new UserData("User2", "User B.Two"), policy);
            _cache.Set("User3", new UserData("User3", "User C.Three"), policy);
        }

        public async Task<IList<Message>> GetChatHistoryAsync(int limit, CancellationToken token)
        {
            await Task.Delay(2000);

            IList<Message> res = new List<Message>();
            res.Add(GetMessage("m1"));
            res.Add(GetMessage("m2"));
            res.Add(GetMessage("m3"));
            res.Add(GetMessage("m4"));
            return res;
        }

        private Message GetMessage(string messageId, bool withreply = true)
        {
            Message res = new Message();
            res.MessageData = _cache.Get(messageId) as MessageData;
            if (res.MessageData == null)
                return null;
            res.UserData = _cache.Get(res.MessageData.SenderId) as UserData;

            if(res.MessageData.ReplyToMessageId != null && withreply)
            {
                res.ReplyToMessage = GetMessage(res.MessageData.ReplyToMessageId, false);
            }
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
            {
                string mesId = Guid.NewGuid().ToString();
                _cache.Add(mesId, new MessageData(mesId, "User3", DateTime.Now, true, new MessageTextContent(textContent.Text), replyToMessageId), null);
                upd.Run(GetMessage(mesId));
            }
            return Task.FromResult(true);
        }


        private MemoryCache _cache = MemoryCache.Default;
        private Message GetMessage(string messageId)
        {
            Message res = new Message();
            res.MessageData = _cache.Get(messageId) as MessageData;
            if (res.MessageData == null)
                return null;
            res.UserData = _cache.Get(res.MessageData.SenderId) as UserData;

            if (res.MessageData.ReplyToMessageId != null)
            {
                res.ReplyToMessage = GetMessage(res.MessageData.ReplyToMessageId);
            }
            return res;
        }
    }

    public class MockMessageUpdater : IMessageUpdater
    {
        private readonly ConcurrentDictionary<object, Action<Message>> _dict =
            new ConcurrentDictionary<object, Action<Message>>();

        public void Run(Message message)
        {
            foreach(var kv in _dict)
            {
                kv.Value.Invoke(message);
            }
        }

        public void Register(object obj, Action<Message> action)
        {
            if(!_dict.ContainsKey(obj))
            {
                _dict.TryAdd(obj, action);
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
        public Task<UserData> GetMeAsync()
        {
            throw new NotImplementedException();
        }

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
