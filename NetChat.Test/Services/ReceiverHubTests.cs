using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NetChat.Desktop.Repository;
using NetChat.Desktop.Services.Messaging;
using NetChat.Desktop.Services.Messaging.Users;
using NUnit.Framework;

namespace NetChat.Test.Services
{
    [TestFixture]
    public class ReceiverHubTests
    {
        private readonly Mock<IMessageReceiver> _messageReceiver;
        private readonly Mock<IUserLoader> _userLoader;

        [Test]
        public void s()
        {
            
            var hub = new ReceiverHub(_messageReceiver.Object, _userLoader.Object);
        }
    } 
}
