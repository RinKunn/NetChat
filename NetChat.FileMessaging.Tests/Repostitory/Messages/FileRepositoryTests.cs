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
        private ITextMessageDataRepository _repo;
        private string _filename;

        [SetUp]
        public void Init()
        {
            var _dir = Path.Combine(Directory.GetCurrentDirectory());
            var _encoding = Encoding.UTF8;
            var config = new RepositoriesConfig(Path.Combine(_dir, Guid.NewGuid().ToString() + ".datnetchat"), _encoding);
            _repo = new TextMessageDataFileRepository(config);
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
                    (new TextMessageData("user11", "hello1")).ToString(),
                    (new TextMessageData("user21", "hello2")).ToString(),
                    (new TextMessageData("user31", "hello3")).ToString(),
                    (new TextMessageData("user41", "hello4")).ToString(),
                    (new TextMessageData("user51", "hello5")).ToString(),
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
                    (new TextMessageData("user1", "hello1")).ToString(),
                    (new TextMessageData("user2", "hello2")).ToString(),
                    (new TextMessageData("user3", "hello3")).ToString(),
                    (new TextMessageData("user4", "hello4")).ToString(),
                    (new TextMessageData("user5", "hello5")).ToString(),
                });

            var res = await _repo.GetAsync(10);

            Assert.AreEqual(5, res.Count);
            Assert.AreEqual("USER1", res[0].UserName);
            Assert.AreEqual("USER5", res[4].UserName);
        }
    }
}
