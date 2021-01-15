using System.Threading.Tasks;
using GalaSoft.MvvmLight.Threading;
using Moq;
using NetChat.Desktop.Services.Messaging;
using NetChat.Desktop.Services.Messaging.Users;
using NetChat.Desktop.ViewModel.Commands;
using NetChat.Desktop.ViewModel.Messenger;
using NUnit.Framework;

namespace NetChat.Test.ViewModel.MessengerArea
{
    [TestFixture]
    public class HeaderViewModelTests
    {
        private Mock<IUserLoader> userLoaderMock;
        private Mock<IReceiverHub> receiverHubMock;
        private Mock<IReceiverHub> receiverHubMock2;
        
        [OneTimeSetUp]
        public void Init()
        {
            DispatcherHelper.Initialize();
            userLoaderMock = new Mock<IUserLoader>();
            userLoaderMock.Setup(s => s.OnlineUsersCount()).Returns(Task.FromResult(3));
            receiverHubMock = new Mock<IReceiverHub>();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            DispatcherHelper.Reset();
        }

        [Test]
        public async Task LoadViewModel_ParticipantOnlineCountEq3()
        {
            var vm = new HeaderViewModel(userLoaderMock.Object, receiverHubMock.Object);

            await vm.LoadCommand.ExecuteAsync(null);
            
            Assert.AreEqual(3, vm.ParticipantOnlineCount);
        }

        [Test]
        public async Task UserLoggedIn_ParticipantOnlineCountEq4()
        {
            var vm = new HeaderViewModel(userLoaderMock.Object, receiverHubMock.Object);

            

            Assert.AreEqual(4, vm.ParticipantOnlineCount);
        }

        [Test]
        public async Task UserLoggedOut_ParticipantOnlineCountEq2()
        {
            var vm = new HeaderViewModel(userLoaderMock.Object, receiverHubMock.Object);

            

            Assert.AreEqual(2, vm.ParticipantOnlineCount);
        }
    }
}
