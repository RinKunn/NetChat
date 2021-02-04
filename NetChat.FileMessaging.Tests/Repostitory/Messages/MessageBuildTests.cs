using System;
using NUnit.Framework;
using NetChat.FileMessaging.Repository.Messages;

namespace NetChat.FileMessaging.Tests.Repostitory.Messages
{
    [TestFixture]
    public class MessageBuildTests
    {
        [Test]
        public void Parse_CleanLine()
        {
            string line = "24.08 17:00:47|5cc62> Hello! My name is '5cc62'";
            TextMessageData mes = TextMessageData.Parse(line);

            Assert.AreEqual(new DateTime(DateTime.Today.Year, 08, 24, 17, 0, 47), mes.DateTime);
            Assert.AreEqual("5cc62", mes.UserName);
            Assert.AreEqual("Hello! My name is '5cc62'", mes.Text);
        }

        [Test]
        public void Parse_NameWithWhiteSpaces()
        {
            string line = "24.08 17:00:47|FGHFF GH dfdf> Hello! My name is '5cc62'";
            TextMessageData mes = TextMessageData.Parse(line);

            Assert.AreEqual(new DateTime(DateTime.Today.Year, 08, 24, 17, 0, 47), mes.DateTime);
            Assert.AreEqual("FGHFF GH dfdf", mes.UserName);
            Assert.AreEqual("Hello! My name is '5cc62'", mes.Text);
        }

        [Test]
        public void Parse_MessageWithGreaterThanSymbol()
        {
            string line = "24.08 17:00:47|FGHFF GH dfdf> Hello! My name > is '5cc > 62'";
            TextMessageData mes = TextMessageData.Parse(line);

            Assert.AreEqual(new DateTime(DateTime.Today.Year, 08, 24, 17, 0, 47), mes.DateTime);
            Assert.AreEqual("FGHFF GH dfdf", mes.UserName);
            Assert.AreEqual("Hello! My name > is '5cc > 62'", mes.Text);
        }

        [Test]
        public void Build_MessageWithNewLines()
        {
            string message = "Hello! My \nname\n is 'Timch'";
            string username = "Timch";
            DateTime dateTime = DateTime.Now;

            TextMessageData mes = new TextMessageData(username, message);

            Assert.AreEqual("Hello! My name is 'Timch'", mes.Text);

        }

        [Test]
        public void Build_NameIsUpperCase()
        {
            string message = "Test message";
            string username = "Timch_Q";
            DateTime dateTime = DateTime.Now;

            TextMessageData mes = new TextMessageData(username, message);

            Assert.AreEqual("TIMCH_Q", mes.UserName);
        }

        [Test]
        public void Build_IdIsCorrect()
        {
            string message = "Test message";
            string username = "Timch_Q";
            DateTime dateTime = DateTime.Now;

            TextMessageData mes = new TextMessageData(username, message);

            Assert.AreEqual($"{dateTime:dd.MM HH:mm:ss}|TIMCH_Q", mes.Id);
        }

        [Test]
        public void Build_NameLongerPadding()
        {
            string message = "Test message";
            string username = "VeryLongNameWhohasthatlongname?";

            TextMessageData mes = new TextMessageData(username, message);

            Assert.AreEqual("VERYLONGNAMEWHOHASTHATLONGNAME?", mes.UserName);
        }
    }
}
