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
    [TestFixture]
    public class ChatSenderViewModelTests
    {
        [Test] public void SendMessage_MessageSendedViaVMMessenger() { }
        [Test] public void SendMessage_InnerError_SendExceptionSendedViaVMMessenger() { }
    }
}
