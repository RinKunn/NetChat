using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using NetChat.FileMessaging.Repository;
using NetChat.FileMessaging.Repository.Messages;
using NetChat.FileMessaging.Repository.UserStatuses;
using NetChat.FileMessaging.Services.Users;
using NUnit.Framework;

namespace NetChat.FileMessaging.Tests.Services
{
    [TestFixture]
    public class UserServiceTests
    {
        private IFileUserService _userService;
        private string _fname;

        [SetUp]
        public void Init()
        {
            var config = new RepositoriesConfig(
                Path.Combine(Directory.GetCurrentDirectory(), Guid.NewGuid().ToString() + ".datnetchat"), Encoding.UTF8);
            _fname = config.MessagesSourcePath;
            var repo = new UserStatusFileRepository(config);
            _userService = new DefaultUserService(repo);
        }

        [TearDown]
        public void TearDonw()
        {
            File.Delete(_fname);
        }

        [Test]
        public async Task GetUsers()
        {
            File.AppendAllLines(_fname,
                new string[]
                {
                    (new MessageDataEntity("user2", "Logout", DateTime.Now.AddSeconds(-4))).ToString(),
                    (new MessageDataEntity("user1", "Bi", DateTime.Now.AddSeconds(-3))).ToString(),
                    (new MessageDataEntity("user2", "Bi", DateTime.Now.AddSeconds(-2))).ToString(),
                    (new MessageDataEntity("user1", "Logout", DateTime.Now.AddSeconds(-1))).ToString(),
                });

            var res = await _userService.GetUsers();

            Assert.AreEqual(2, res.Count);
            Assert.AreEqual("USER1", res[0].Id);
            Assert.IsFalse(res[0].IsOnline);
            Assert.AreEqual("USER2", res[1].Id);
            Assert.IsTrue(res[1].IsOnline);
        }
    }
}
