//using System;
//using System.Threading.Tasks;
//using GalaSoft.MvvmLight.Threading;
//using Moq;
////using NetChat.Desktop.Commands;
//using NetChat.Services.Messaging.Users;
//using NetChat.Desktop.ViewModel.Messenger;
//using NetChat.Test.Moqs;
//using NUnit.Framework;

//namespace NetChat.Test.ViewModel.MessengerArea
//{
//    [TestFixture]
//    public class HeaderViewModelTests
//    {
//        private Mock<IUserLoader> _userLoaderMock;
//        private ReceiverHubMock _receiverHubMock;
        
//        [SetUp]
//        public void Init()
//        {
//            DispatcherHelper.Initialize();
//            _userLoaderMock = new Mock<IUserLoader>();
//            _userLoaderMock.Setup(s => s.OnlineUsersCount()).Returns(Task.FromResult(3));
//            _receiverHubMock = new ReceiverHubMock();
//            _receiverHubMock.Connect();
//        }

//        [TearDown]
//        public void TearDown()
//        {
//            DispatcherHelper.Reset();
//            _receiverHubMock.Disconnect();
//        }

//        [Test]
//        public async Task Load_ParticipantOnlineCountEq3()
//        {
//            var vm = new ChatHeaderViewModel(_userLoaderMock.Object, _receiverHubMock);

//            await vm.LoadCommand.ExecuteAsync(null);
            
//            Assert.AreEqual(3, vm.ParticipantOnlineCount);
//        }

//        [Test]
//        public void LoadThrow_s()
//        {
//            _userLoaderMock.Setup(s => s.OnlineUsersCount()).Throws<Exception>();
//            var vm = new ChatHeaderViewModel(_userLoaderMock.Object, _receiverHubMock);

//            vm.LoadCommand.ExecuteAsync(null);

//            Assert.True(((AsyncCommand)vm.LoadCommand).Execution.IsFaulted);
//            Assert.False(((AsyncCommand)vm.LoadCommand).Execution.IsSuccessfullyCompleted);
//        }

//        [Test]
//        public async Task UserLoggedIn_ParticipantOnlineCountEq4()
//        {
//            var vm = new ChatHeaderViewModel(_userLoaderMock.Object, _receiverHubMock);
//            await vm.LoadCommand.ExecuteAsync(null);

//            _receiverHubMock.RaiseParticipantReceived(new ParticipantObservable("u", true, DateTime.Now));

//            Assert.AreEqual(4, vm.ParticipantOnlineCount);
//        }

//        [Test]
//        public async Task UserLoggedOut_ParticipantOnlineCountEq2()
//        {
//            var vm = new ChatHeaderViewModel(_userLoaderMock.Object, _receiverHubMock);
//            await vm.LoadCommand.ExecuteAsync(null);

//            _receiverHubMock.RaiseParticipantReceived(new ParticipantObservable("u", false, DateTime.Now));

//            Assert.AreEqual(2, vm.ParticipantOnlineCount);
//        }
//    }
//}
