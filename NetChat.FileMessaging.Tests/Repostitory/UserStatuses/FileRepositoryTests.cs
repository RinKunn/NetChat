using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using NetChat.FileMessaging.Repository;
using NetChat.FileMessaging.Repository.Messages;
using NetChat.FileMessaging.Repository.UserStatuses;
using NUnit.Framework;

namespace NetChat.FileMessaging.Tests.Repostitory.UserStatuses
{
    [TestFixture]
    public class FileRepositoryTests
    {
        private IUserStatusRepository _repo;
        private string _filename;

        [SetUp]
        public void Init()
        {
            var _dir = Path.Combine(Directory.GetCurrentDirectory());
            var _encoding = Encoding.UTF8;
            var config = new RepositoriesConfig(Path.Combine(_dir, Guid.NewGuid().ToString() + ".datnetchat"), _encoding);
            _repo = new UserStatusFileRepository(config);
            _filename = config.MessagesSourcePath;
        }

        [TearDown]
        public void TearDown()
        {
            File.Delete(_filename);
        }

        [Test]
        public async Task SetUserStatusOnline()
        {
            await _repo.SetUserStatus(new UserStatus()
            { 
                UserId = "User1", 
                IsOnline = true,
                UpdateDateTime = DateTime.Now
            });

            var lines = File.ReadAllLines(_filename);

            var m = new TextMessageData(lines[0]);
            Assert.AreEqual(1, lines.Length);
            Assert.AreEqual("Logon", m.Text);
            Assert.AreEqual("USER1", m.UserName);
        }

        [Test]
        public async Task SetUserStatusOffline()
        {
            await _repo.SetUserStatus(new UserStatus()
            {
                UserId = "User1",
                IsOnline = false,
                UpdateDateTime = DateTime.Now
            });

            var lines = File.ReadAllLines(_filename);

            var m = new TextMessageData(lines[0]);
            Assert.AreEqual(1, lines.Length);
            Assert.AreEqual("Logout", m.Text);
            Assert.AreEqual("USER1", m.UserName);
        }
    }
}
