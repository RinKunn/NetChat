using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using Moq;
using NetChat.Desktop.Services.Messaging;
using NetChat.Desktop.Services.Messaging.Messages;
using NetChat.Desktop.Services.Messaging.Users;
using NetChat.Desktop.ViewModel.Commands;
using NetChat.Desktop.ViewModel.InnerMessages;
using NetChat.Desktop.ViewModel.Messenger;
using NetChat.Test.Moqs;
using NUnit.Framework;

namespace NetChat.Test.ViewModel.Messenger
{
    internal static class TestsHelper
    {
        internal static IList<ParticipantObservable> GenerateParticipantList(int count)
        {
            if (count <= 0) throw new ArgumentNullException(nameof(count));
            var res = new List<ParticipantObservable>();
            for (int i = 0; i < count; i++)
            {
                res.Add(GenerateParticipant(i));
            }
            return res;
        }
        internal static ParticipantObservable GenerateParticipant(int userKey)
        {
            if (userKey < 0) throw new ArgumentNullException(nameof(userKey));
            return new ParticipantObservable(
                    "User" + userKey,
                    userKey % 2 == 0 ? true : false,
                    DateTime.Now);
        }
        internal static TextMessageObservable GenerateMessage(int messKey, int userKey, bool isOriginNative = false)
        {
            if (messKey < 0) throw new ArgumentNullException(nameof(messKey));
            if (userKey < 0) throw new ArgumentNullException(nameof(userKey));
            return new TextMessageObservable(
                    "Message" + messKey,
                    messKey.ToString(),
                    DateTime.Now,
                    GenerateParticipant(userKey),
                    isOriginNative);
        }
        internal static TextMessageObservable GenerateMessage(int messKey, ParticipantObservable user, bool isOriginNative = false)
        {
            if (messKey < 0) throw new ArgumentNullException(nameof(messKey));
            if (user == null) throw new ArgumentNullException(nameof(user));
            return new TextMessageObservable(
                    "Message" + messKey,
                    messKey.ToString(),
                    DateTime.Now,
                    user,
                    isOriginNative);
        }
        internal static IList<TextMessageObservable> GenerateMessageList(int messCount, int usersCount)
        {
            if (messCount <= 0) throw new ArgumentNullException(nameof(messCount));
            if (usersCount <= 0) throw new ArgumentNullException(nameof(usersCount));
            var users = GenerateParticipantList(usersCount);
            var res = new List<TextMessageObservable>();
            for (int i = 0; i < messCount; i++)
            {
                res.Add(GenerateMessage(i, users[i % usersCount], false));
            }
            return res;
        }
        internal static void FillMessages(ChatAreaViewModel vm, int count, int usersCount)
        {
            var mess = GenerateMessageList(count, usersCount);
            for (int i = 0; i < mess.Count; i++)
                vm.Messages.Add(mess[i]);
        }
    }

    [TestFixture]
    public class ChatAreaViewModelTests
    {
        private Mock<IMessenger> _vmMessengerMock;
        private Mock<IMessageLoader> _messageLoaderMock;
        private ReceiverHubMock _receiverHub;

        [SetUp]
        public void Init()
        {
            DispatcherHelper.Initialize();
            _messageLoaderMock = new Mock<IMessageLoader>();
            _vmMessengerMock = new Mock<IMessenger>();
            _receiverHub = new ReceiverHubMock();
            _receiverHub.Connect();
        }
        [TearDown]
        public void Cleanup()
        {
            DispatcherHelper.Reset();
            _receiverHub.Disconnect();
        }

        [Test] public void Init_MessageLoaderIsNull_ThrowArgNulExc()
        {
            Assert.Throws<ArgumentNullException>(() => 
                new ChatAreaViewModel(
                    null, 
                    _receiverHub, 
                    _vmMessengerMock.Object));
        }
        [Test] public void Init_ReceiverHubIsNull_ThrowArgNulExc()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new ChatAreaViewModel(
                    _messageLoaderMock.Object, 
                    null, 
                    _vmMessengerMock.Object));
        }
        [Test] public void Init_VMMessengerIsNull_ThrowArgNulExc()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new ChatAreaViewModel(
                    _messageLoaderMock.Object, 
                    _receiverHub, 
                    null));
        }
        [Test] public void Init_SubscribedToMessagesReceiver()
        {
            var receiverHubMock = new Mock<IReceiverHub>();

            var vm = new ChatAreaViewModel(
                _messageLoaderMock.Object,
                receiverHubMock.Object, 
                _vmMessengerMock.Object);

            receiverHubMock.Verify(m => 
                m.SubscribeMessageReceived(vm, It.IsAny<Action<MessageObservable>>()), 
                Times.Once);
        }
        [Test] public void Init_SubscribedToVMGotoMessage()
        {
            var vm = new ChatAreaViewModel(
                _messageLoaderMock.Object,
                _receiverHub,
                _vmMessengerMock.Object);

            _vmMessengerMock.Verify(m =>
                m.Register<GoToMessageInnerMessage>(vm, It.IsAny<Action<GoToMessageInnerMessage>>(), false),
                Times.Once);
        }
        [Test] public void CleanUp_UnsubscribedFromMessagesReceiver()
        {
            var receiverHubMock = new Mock<IReceiverHub>();
            var vm = new ChatAreaViewModel(
                _messageLoaderMock.Object,
                receiverHubMock.Object,
                _vmMessengerMock.Object);
            
            vm.Cleanup();

            receiverHubMock.Verify(m =>
                m.UnsubscribeMessageReceived(vm),
                Times.Once);
        }
        [Test] public void CleanUp_UnsubscribedFromVMGotoMessage()
        {
            var vm = new ChatAreaViewModel(
                _messageLoaderMock.Object,
                _receiverHub,
                _vmMessengerMock.Object);

            vm.Cleanup();

            _vmMessengerMock.Verify(m =>
                m.Unregister<GoToMessageInnerMessage>(vm),
                Times.Once);
        }
        [Test] public void CleanUp_MessagesIsEmpty()
        {
            var vm = new ChatAreaViewModel(
                _messageLoaderMock.Object,
                _receiverHub,
                _vmMessengerMock.Object);
            TestsHelper.FillMessages(vm, 3, 2);

            vm.Cleanup();

            Assert.IsEmpty(vm.Messages);
        }

        [Test] public void AddMessNotNative_ScrollUp_UnreadMessEncr()
        {
            var vm = new ChatAreaViewModel(
                _messageLoaderMock.Object, 
                _receiverHub, 
                _vmMessengerMock.Object);
            TestsHelper.FillMessages(vm, 3, 2);
            vm.LastVisibleMessageIndex = 1;

            vm.Messages.Add(TestsHelper.GenerateMessage(3, 2, false));

            Assert.AreEqual(1, vm.LastVisibleMessageIndex);
            Assert.AreEqual(1, vm.NewMessagesCount);
        }
        [Test] public void AddMessNotNative_ScrollDown_NoUnreadMess() { }
        [Test] public void AddMessNative_ScrollUp_NoUnreadAndScrollDown() { }
        [Test] public void AddMessNative_ScrollDown_NoUnreadMess() { }
        [Test] public void UpBy1FromLastUnreadMess_HasNewMess_UnreadMessCountNotChanged() { }
        [Test] public void DownBy1FromLastUnreadMess_HasNewMess_UnreadMessCountDescrBy1() { }
        [Test] public void DownBy1UpperLastUnreadMess_HasNewMessages_UnreadMessCountNotChanged() { }

        [Test] public void LoadMessCmd_HubConn_MessLoaded() { }
        [Test] public void LoadMessCmd_HubDisc_ErrMessSendedThrowVMMessenger() { }
        [Test] public void LoadMessCmd_HubThrowExc_ErrMessSendedThrowVMMessenger() { }

        [Test] public void ReadAllMessCmd_UnreadMessEmpty_CannotExecute() { }
        [Test] public void ReadAllMessCmd_HasUnreadMess_UnreadMessagesEmpty() { }
    }
}
