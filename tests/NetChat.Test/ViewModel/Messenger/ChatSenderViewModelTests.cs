using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using Moq;
using NetChat.Services.Messaging;
using NetChat.Services.Messaging.Messages;
using NetChat.Services.Messaging.Users;
using NetChat.Desktop.Commands;
using NetChat.Desktop.InnerMessages;
using NetChat.Desktop.ViewModel.Messenger;
using NetChat.Test.Moqs;
using NUnit.Framework;

namespace NetChat.Test.ViewModel.Messenger
{
    [TestFixture]
    public class ChatSenderViewModelTests
    {
        [Test] public void SendMessage_MessageSendedViaVMMessenger() { }
        [Test] public void SendMessage_InnerError_SendExceptionSendedViaVMMessenger() { }
    }
}
