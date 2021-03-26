using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using NetChat.FileMessaging.Repository;
using NetChat.FileMessaging.Repository.Messages;
using NUnit.Framework;

namespace NetChat.FileMessaging.Tests.Repostitory.Messages
{
    [TestFixture]
    public class FileRepositoryTests
    {
        private IMessageDataEntityRepository _repo;
        private string _filename;

        [SetUp]
        public void Init()
        {
            var _dir = Path.Combine(Directory.GetCurrentDirectory());
            var _encoding = Encoding.UTF8;
            var config = new RepositoriesConfig(Path.Combine(_dir, Guid.NewGuid().ToString() + ".datnetchat"), _encoding);
            _repo = new MessageDataEntityFileRepository(config);
            _filename = config.MessagesSourcePath;
        }

        [TearDown]
        public void TearDown()
        {
            File.Delete(_filename);
        }

        [Test]
        public async Task GetLast2StringFrom5_Return2()
        {
            File.AppendAllLines(_filename, 
                new string[]
                {
                    (new MessageDataEntity("user11", "hello1")).ToString(),
                    (new MessageDataEntity("user21", "hello2")).ToString(),
                    (new MessageDataEntity("user31", "hello3")).ToString(),
                    (new MessageDataEntity("user41", "hello4")).ToString(),
                    (new MessageDataEntity("user51", "hello5")).ToString(),
                });

            var res = await _repo.GetAsync(2);

            Assert.AreEqual(2, res.Count);
            Assert.AreEqual("USER41", res[0].UserName);
            Assert.AreEqual("USER51", res[1].UserName);
        }

        [Test]
        public async Task GetLast10StringFrom5_Return5()
        {
            File.AppendAllLines(_filename,
                new string[]
                {
                    (new MessageDataEntity("user1", "hello1")).ToString(),
                    (new MessageDataEntity("user2", "hello2")).ToString(),
                    (new MessageDataEntity("user3", "hello3")).ToString(),
                    (new MessageDataEntity("user4", "hello4")).ToString(),
                    (new MessageDataEntity("user5", "hello5")).ToString(),
                });

            var res = await _repo.GetAsync(10);

            Assert.AreEqual(5, res.Count);
            Assert.AreEqual("USER1", res[0].UserName);
            Assert.AreEqual("USER5", res[4].UserName);
        }

        [Test]
        public async Task GetAllAsync_WithBadFormatLine_IgnoreBadLine()
        {
            File.AppendAllLines(_filename,
                new string[]
                {
                    (new MessageDataEntity("user1", "hello1")).ToString(),
                    "BAD LINE",
                    (new MessageDataEntity("user3", "hello3")).ToString(),
                });

            var res = await _repo.GetAllAsync();

            Assert.AreEqual(2, res.Count);
            Assert.AreEqual("USER1", res[0].UserName);
            Assert.AreEqual("USER3", res[1].UserName);
        }

        [Test]
        public async Task GetAllAsync_WithReplyId()
        {
            File.AppendAllLines(_filename,
                new string[]
                {
                    (new MessageDataEntity("user1", "hello1", DateTime.Now, "11.11.2020 11:11:11|HELL1")).ToString(),
                    (new MessageDataEntity("user3", "hello3")).ToString(),
                });

            var res = await _repo.GetAllAsync();

            Assert.AreEqual(2, res.Count);
            Assert.AreEqual("USER1", res[0].UserName);
            Assert.AreEqual("11.11.2020 11:11:11|HELL1", res[0].ReplyToMessageId);
            Assert.AreEqual("USER3", res[1].UserName);
            Assert.IsNull(res[1].ReplyToMessageId);
        }

        [Test]
        public async Task GetAllAsync_OneLineIsLogonMess_IgnoreLine()
        {
            File.AppendAllLines(_filename,
                new string[]
                {
                    (new MessageDataEntity("user1", "hello1")).ToString(),
                    (new MessageDataEntity("user1", "Logon")).ToString(),
                    (new MessageDataEntity("user3", "hello3")).ToString(),
                });

            var res = await _repo.GetAllAsync();

            Assert.AreEqual(2, res.Count);
            Assert.AreEqual("USER1", res[0].UserName);
            Assert.AreEqual("USER3", res[1].UserName);
        }

        [Test]
        public async Task GetAllAsync_OneLineIsLogoutMess_IgnoreLine()
        {
            File.AppendAllLines(_filename,
                new string[]
                {
                    (new MessageDataEntity("user1", "hello1")).ToString(),
                    (new MessageDataEntity("user1", "Logout")).ToString(),
                    (new MessageDataEntity("user3", "hello3")).ToString(),
                });

            var res = await _repo.GetAllAsync();

            Assert.AreEqual(2, res.Count);
            Assert.AreEqual("USER1", res[0].UserName);
            Assert.AreEqual("USER3", res[1].UserName);
        }
    }
}
