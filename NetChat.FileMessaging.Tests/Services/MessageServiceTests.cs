using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using System.Linq;
using NetChat.FileMessaging.Models;
using NetChat.FileMessaging.Repository.Messages;
using NetChat.FileMessaging.Services.Messages;
using NUnit.Framework;

namespace NetChat.FileMessaging.Tests.Services
{
    [TestFixture]
    public class MessageServiceTests
    {
        private IMessageService _messageService;
        private  Mock<IMessageDataEntityRepository> _repositoryMock;
        private IList<MessageDataEntity> _data;

        [SetUp]
        public void Init()
        {
            _repositoryMock = new Mock<IMessageDataEntityRepository>();
            _repositoryMock
                .Setup(m => m.AddAsync(It.IsAny<MessageDataEntity>()))
                .Returns<MessageDataEntity>(m => Task.FromResult(m));
            _repositoryMock
                .Setup(m => m.AddSomeAsync(It.IsAny<IEnumerable<MessageDataEntity>>()))
                .Returns<IEnumerable<MessageDataEntity>>(m => Task.FromResult(m));
            
            _messageService = new DefaultMessageService(_repositoryMock.Object);
        }
        [TearDown]
        public void ClearUp()
        {
            _repositoryMock = null;
            _messageService = null;
            _data?.Clear();
        }

        [Test]
        public void SendMessage_MessageIsNotNull_AddedToRepo()
        {
            var sendingMessageData = new InputMessageData("User1", "Text", "01.01.2020 11:22:33|USER2");
            MessageData res = null;

            Assert.DoesNotThrowAsync(async () => res = await _messageService.SendMessage(sendingMessageData));

            Assert.NotNull(res);
            Assert.NotNull(res.Id);
            Assert.AreEqual("USER1", res.SenderId);
            Assert.AreEqual("Text", res.Text);
            Assert.AreEqual("01.01.2020 11:22:33|USER2", res.ReplyToMessageId);
            
            _repositoryMock.Verify(m => m.AddAsync(
                It.Is<MessageDataEntity>(d => d.Text == "Text")), Times.Once);
            _repositoryMock.Verify(
                m => m.AddSomeAsync(It.IsAny<IEnumerable<MessageDataEntity>>()),
                Times.Never);
        }
        [Test]
        public void SendMessage_MessageIsNull_ThrowArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() 
                => _messageService.SendMessage(new InputMessageData("User1", null, null)));

            _repositoryMock.Verify(
                m => m.AddAsync(It.IsAny<MessageDataEntity>()),
                Times.Never);
        }
        [Test]
        public void SendMessage_ReplyIdHasWrongFormat_ThrowFormatException()
        {
            Assert.ThrowsAsync<FormatException>(() 
                => _messageService.SendMessage(new InputMessageData("User1", "Text", "BADREPLY")));
            _repositoryMock.Verify(
                m => m.AddAsync(It.IsAny<MessageDataEntity>()),
                Times.Never);
        }
        [Test]
        public void SendMessage_MessageContainsNewLine_AddedSplittedMessagesToRepo()
        {
            var sendingMessageData = new InputMessageData("User1", "Text1\nText2\nText3", "01.01.2020 11:22:33|USER2");
            MessageData res = null;

            Assert.DoesNotThrowAsync(async () => res = await _messageService.SendMessage(sendingMessageData));

            Assert.NotNull(res);
            Assert.NotNull(res.Id);
            Assert.AreEqual("USER1", res.SenderId);
            Assert.AreEqual("Text1\nText2\nText3", res.Text);
            Assert.AreEqual("01.01.2020 11:22:33|USER2", res.ReplyToMessageId);

            _repositoryMock.Verify(m 
                => m.AddAsync(It.IsAny<MessageDataEntity>()), 
                Times.Never);
            _repositoryMock.Verify(m
                => m.AddSomeAsync(It.IsAny<IEnumerable<MessageDataEntity>>()), 
                Times.Once);
            _repositoryMock.Verify(m
                => m.AddSomeAsync(It.Is<IEnumerable<MessageDataEntity>>(d 
                    => d.Count() == 3 
                       && d.First().Text == "Text1"
                       && d.Last().Text == "Text3")),
                Times.Once);
        }

        [Test]
        public void LoadMessagesAsync_FromDifferUsers_ReturnDifferMess()
        {
            _data = new List<MessageDataEntity>()
            {
                new MessageDataEntity("User1", "Text From User1"),
                new MessageDataEntity("User2", "Text From User2"),
                new MessageDataEntity("User3", "Text From User3"),
                new MessageDataEntity("User4", "Text From User4"),
            };
            _repositoryMock
                .Setup(m => m.GetAsync(It.IsAny<int>(), CancellationToken.None))
                .Returns(Task.FromResult(_data));

            IList<MessageData> messages = null;
            Assert.DoesNotThrowAsync(async () => messages = await _messageService.LoadMessagesAsync());

            Assert.NotNull(messages);
            Assert.AreEqual(4, messages.Count);
            Assert.AreEqual("USER1", messages[0].SenderId);
            Assert.AreEqual("USER4", messages[3].SenderId);
        }

        [Test]
        public void LoadMessagesAsync_FromOneUserAtDifferTime_ReturnDifferMess()
        {
            _data = new List<MessageDataEntity>()
            {
                new MessageDataEntity("User", "Text1 From User", DateTime.Now.AddMinutes(1)),
                new MessageDataEntity("User", "Text2 From User", DateTime.Now.AddMinutes(2)),
                new MessageDataEntity("User", "Text3 From User", DateTime.Now.AddMinutes(3)),
                new MessageDataEntity("User", "Text4 From User", DateTime.Now.AddMinutes(4)),
            };
            _repositoryMock
                .Setup(m => m.GetAsync(It.IsAny<int>(), CancellationToken.None))
                .Returns(Task.FromResult(_data));
            IList<MessageData> messages = null;
            Assert.DoesNotThrowAsync(async () => messages = await _messageService.LoadMessagesAsync());

            Assert.NotNull(messages);
            Assert.AreEqual(4, messages.Count);
            Assert.AreEqual("USER", messages[0].SenderId);
            Assert.AreEqual("USER", messages[3].SenderId);
        }

        [Test]
        public void LoadMessagesAsync_FromOneUserAtOneTime_ReturnOneMessage()
        {
            var date = DateTime.Now;
            _data = new List<MessageDataEntity>()
            {
                new MessageDataEntity("User2", "Text1 From User2", date),
                new MessageDataEntity("User", "Text1 From User", date),
                new MessageDataEntity("User", "Text2 From User", date),
                new MessageDataEntity("User", "Text3 From User", date),
                new MessageDataEntity("User1", "Text1 From User1", date),
            };
            _repositoryMock
                .Setup(m => m.GetAsync(It.IsAny<int>(), CancellationToken.None))
                .Returns(Task.FromResult(_data));
            IList<MessageData> messages = null;
            Assert.DoesNotThrowAsync(async () => messages = await _messageService.LoadMessagesAsync());

            Assert.NotNull(messages);
            Assert.AreEqual(3, messages.Count);
            Assert.AreEqual("USER2", messages[0].SenderId);
            Assert.AreEqual("USER", messages[1].SenderId);
            Assert.AreEqual("Text1 From User\nText2 From User\nText3 From User", messages[1].Text);
            Assert.AreEqual("USER1", messages[2].SenderId);
            Assert.AreEqual("Text1 From User1", messages[2].Text);
        }
    }
}
