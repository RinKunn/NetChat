using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NetChat.FileMessaging.Repository;
using NetChat.FileMessaging.Repository.Messages;
using NetChat.FileMessaging.Repository.UserStatuses;
using NetChat.FileMessaging.Services;
using NetChat.FileMessaging.Services.Users;
using NLog;
using NLog.Config;
using NUnit.Framework;

namespace NetChat.FileMessaging.Tests.Services
{
    [TestFixture]
    public class FileNetChatHubTests
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly INetChatHub _hub;
        private readonly string _filename;

        public FileNetChatHubTests()
        {
            _filename = Path.Combine(Directory.GetCurrentDirectory(), "nettest.txt".ToString());
            _hub = new FileNetChatHub(new RepositoriesConfig(_filename, Encoding.UTF8));
        }

        [TearDown]
        public void TearDown()
        {
            if (_hub.IsConnected)
                _hub.Disconnect();
        }

        [Test]
        public void MessageSend_HanlerInvoked()
        {   
            OnMessageReceivedArgs raisedEventArgs = null;
            ManualResetEvent statsUpdatedEvent = new ManualResetEvent(false);
            string user = "User1";
            string text = Guid.NewGuid().ToString();
            Task _hub_OnMessageReceived(OnMessageReceivedArgs message)
            {
                raisedEventArgs = message;
                statsUpdatedEvent.Set();
                return Task.CompletedTask;
            }
            _hub.OnMessageReceived += _hub_OnMessageReceived;
            _hub.Connect();

            File.AppendAllText(_filename, (new MessageDataEntity(user, text)).ToString() + "\n");
            statsUpdatedEvent.WaitOne(500, false);

            Assert.NotNull(raisedEventArgs);
            Assert.AreEqual(user.ToUpper(), raisedEventArgs.Message.SenderId);
            Assert.AreEqual(text, raisedEventArgs.Message.Text);
        }

        [Test]
        public void MessageSend_AsyncHanlerInvoked()
        {
            OnMessageReceivedArgs raisedEventArgs = null;
            ManualResetEvent statsUpdatedEvent = new ManualResetEvent(false);
            string user = "User1";
            string text = Guid.NewGuid().ToString();
            async Task _hub_OnMessageReceived(OnMessageReceivedArgs message)
            {
                raisedEventArgs = message;
                await Task.Delay(100);
                statsUpdatedEvent.Set();
            }
            _hub.OnMessageReceived += _hub_OnMessageReceived;
            _hub.Connect();

            File.AppendAllText(_filename, (new MessageDataEntity(user, text)).ToString() + "\n");
            statsUpdatedEvent.WaitOne(500, false);

            Assert.NotNull(raisedEventArgs);
            Assert.AreEqual(user.ToUpper(), raisedEventArgs.Message.SenderId);
            Assert.AreEqual(text, raisedEventArgs.Message.Text);
        }

        [Test]
        public void FileAppendTextOnDisc_NothingRaised()
        {
            OnMessageReceivedArgs raisedEventArgs = null;
            ManualResetEvent statsUpdatedEvent = new ManualResetEvent(false);
            string user = "User1";
            string text = Guid.NewGuid().ToString();
            Task _hub_OnMessageReceived(OnMessageReceivedArgs message)
            {
                raisedEventArgs = message;
                statsUpdatedEvent.Set();
                return Task.CompletedTask;
            }
            _hub.OnMessageReceived += _hub_OnMessageReceived;
            _hub.Connect();
            _hub.Disconnect();

            File.AppendAllText(_filename, (new MessageDataEntity(user, text)).ToString() + "\n");
            statsUpdatedEvent.WaitOne(500, false);

            Assert.Null(raisedEventArgs);
        }
    }
}
