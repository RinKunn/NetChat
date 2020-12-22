using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Threading.Tasks;
using Moq;
using NetChat.Desktop.Services.Messaging.Users;

namespace NetChat.Test.ViewModel.Messenger
{
    [TestFixture]
    public class HeaderViewModelTests
    {
        private Mock<IUserLoader> userLoaderMock;

        public HeaderViewModelTests()
        {
            var users = new List<User>();
            users.Add(new User() { Id = "UserMe", Status = UserStatus.Online, StatusChangedDateTime = DateTime.Now });
            users.Add(new User() { Id = "UserOffline", Status = UserStatus.Offline, StatusChangedDateTime = DateTime.Now });
            users.Add(new User() { Id = "UserOnline", Status = UserStatus.Online, StatusChangedDateTime = DateTime.Now });
            userLoaderMock = new Mock<IUserLoader>();
            userLoaderMock.Setup(s => s.LoadUsers()).Returns(Task.FromResult((IEnumerable<User>)users));
        }

        [Test]
        public void we()
        {

        }
    }
}
