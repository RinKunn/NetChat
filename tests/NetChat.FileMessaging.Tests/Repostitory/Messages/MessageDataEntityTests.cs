using System;
using NetChat.FileMessaging.Repository.Messages;
using NUnit.Framework;

namespace NetChat.FileMessaging.Tests.Repostitory.Messages
{
    [TestFixture]
    public class MessageDataEntityTests
    {
        [Test]
        public void Build_IdIsCorrect()
        {
            string message = "Test message";
            string username = "Timch_Q";
            DateTime dateTime = DateTime.Now;

            MessageDataEntity mes = new MessageDataEntity(username, message);

            Assert.AreEqual($"{dateTime:dd.MM.yyyy HH:mm:ss}|TIMCH_Q", mes.Id);
        }
        [Test]
        public void Build_MessageIsEmpty_ThrowAgrumentException()
        {
            string message = "";
            string username = "Timch";
            Assert.Throws<ArgumentNullException>(() => new MessageDataEntity(username, message));
        }
        [Test]
        public void Build_MessageIsNull_ThrowAgrumentException()
        {
            string message = null;
            string username = "Timch";
            Assert.Throws<ArgumentNullException>(() => new MessageDataEntity(username, message));
        }
        [Test]
        public void Build_MessageContainsNewline_ThrowFromatException()
        {
            string message = "Hello! My \nname\n is 'Timch'";
            string username = "Timch";

            Assert.Throws<FormatException>(() 
                => new MessageDataEntity(username, message));
        }
        [Test]
        public void Build_MessageContainsExtraSpaces_TextUnchanged()
        {
            string message = "Hello! My   name  is 'Timch'";
            string username = "Timch";

            MessageDataEntity mes = new MessageDataEntity(username, message);

            Assert.AreEqual("Hello! My   name  is 'Timch'", mes.Text);
        }
        [Test]
        public void Build_UserNameIsEmpty_ThrowAgrumentException()
        {
            string message = "Hello! My name is 'Timch'";
            string username = "";

            Assert.Throws<ArgumentNullException>(() => new MessageDataEntity(username, message));
        }
        [Test]
        public void Build_UserNameIsNull_ThrowAgrumentException()
        {
            string message = "Hello! My name is 'Timch'";
            string username = null;

            Assert.Throws<ArgumentNullException>(() => new MessageDataEntity(username, message));
        }
        [Test]
        public void Build_UserNameContainsSpaces_UserNameUnchanged()
        {
            string message = "Hello! My name is 'Timch'";
            string username = "Timch Ah Del s";

            MessageDataEntity mes = new MessageDataEntity(username, message);

            Assert.AreEqual("TIMCH AH DEL S", mes.UserName);
        }
        [Test]
        public void Build_UserNameInLowerCase_ConvertToUpperCase()
        {
            string message = "Hello! My name is 'Timch'";
            string username = "timch";

            MessageDataEntity mes = new MessageDataEntity(username, message);

            Assert.AreEqual("TIMCH", mes.UserName);
        }
        [Test]
        public void Build_ReplyIdIsEmpty_ReplyIdIsNull()
        {
            string message = "Hello! My name is 'Timch'";
            string username = "timch";
            string replyId = "";

            MessageDataEntity mes = new MessageDataEntity(username, message, DateTime.Now, replyId);

            Assert.IsNull(mes.ReplyToMessageId);
        }
        [TestCase("01.02.2020 11:22:33|hf")]
        [TestCase("01.02.2020 11:22:33| HELLO")]
        [TestCase("01.02.2020 11:22:33 HELLO")]
        [TestCase("01.02.2020 11:22|HELLO")]
        [TestCase("01.02.2020 11:22|HELLO")]
        [TestCase("22.22.9999 99:99:99|HELLO")]
        public void Build_ReplyIdBadCases_ThrowFormatException(string replyId)
        {
            string message = "Hello! My name is 'Timch'";
            string username = "timch";

            Assert.Throws<FormatException>(() => new MessageDataEntity(username, message, DateTime.Now, replyId));
        }
        [TestCase("01.02.2020 11:22:33|HELLO")]
        [TestCase("01.02 11:22:33|HELLO")]
        [TestCase("01.02 11:22:33|HELLO_HE")]
        [TestCase("01.02 11:22:33|!WJW")]
        [TestCase("01.02.2020 11:22:33|!WJW")]
        [TestCase("01.02.2020 11:22:33|USER2_SS")]
        public void Build_ReplyIdGoodCases_ReplyIdNotNull(string replyId)
        {
            string message = "Hello! My name is 'Timch'";
            string username = "timch";

            var mes = new MessageDataEntity(username, message, DateTime.Now, replyId);

            Assert.NotNull(mes.ReplyToMessageId);
            Assert.AreEqual(replyId, mes.ReplyToMessageId);
        }

        [Test]
        public void ToString_UserNameLong_UserNameIsNotTruncated()
        {
            string message = "Hello! My name is 'VeryLongNameSurnameLastname'";
            string username = "VeryLongNameSurnameLastname";
            DateTime dateTime = DateTime.Now;
            MessageDataEntity mes = new MessageDataEntity(username, message, dateTime);

            string res = mes.ToString();

            Assert.AreEqual(
                $"{dateTime:dd.MM.yyyy HH:mm:ss}|VERYLONGNAMESURNAMELASTNAME> " +
                    "Hello! My name is 'VeryLongNameSurnameLastname'", 
                res);
        }
        [Test]
        public void ToString_UserNameShort_PaddedWithSpaces()
        {
            string message = "Hello! My name is 'Sn'";
            string username = "Sn";
            DateTime dateTime = DateTime.Now;
            MessageDataEntity mes = new MessageDataEntity(username, message, dateTime);

            string res = mes.ToString();

            Assert.AreEqual(
                $"{dateTime:dd.MM.yyyy HH:mm:ss}|SN        > Hello! My name is 'Sn'",
                res);
        }
        [Test]
        public void ToString_HasReply_ResultHasReplyId()
        {
            string reply_message = "Hello! My name is 'Name1'";
            string reply_username = "Name1";
            DateTime reply_dateTime = DateTime.Now;
            MessageDataEntity reply_mes = 
                new MessageDataEntity(reply_username, reply_message, reply_dateTime);
            
            string origin_message = "Hello! My name is 'Name2'";
            string origin_username = "Name2";
            DateTime origin_dateTime = DateTime.Now;
            MessageDataEntity origin_mes = 
                new MessageDataEntity(origin_username, origin_message, origin_dateTime, reply_mes.Id);

            string res = origin_mes.ToString();

            Assert.AreEqual(
                $"{origin_dateTime:dd.MM.yyyy HH:mm:ss}|NAME2     > " +
                    $"{reply_dateTime:dd.MM.yyyy HH:mm:ss}|NAME1     > " +
                    $"Hello! My name is 'Name2'",
                res);
        }

        [Test]
        public void Parse_EmptyLine_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => MessageDataEntity.Parse(""));
        }
        [Test]
        public void Parse_UserNameShort_ExtraSpacesRemoved()
        {
            string line = "24.08.2011 17:00:47|VI        > Hello! My name is 'Vi'";
            MessageDataEntity mes = MessageDataEntity.Parse(line);

            Assert.AreEqual(new DateTime(2011, 08, 24, 17, 0, 47), mes.DateTime);
            Assert.AreEqual("VI", mes.UserName);
            Assert.AreEqual("Hello! My name is 'Vi'", mes.Text);
        }
        [Test]
        public void Parse_UserNameContainsSpaces_UserNameUnchanged()
        {
            string line = "24.08.2011 17:00:47|FGHFF GH DFDF> Hello! My name is '???'";
            MessageDataEntity mes = MessageDataEntity.Parse(line);

            Assert.AreEqual(new DateTime(2011, 08, 24, 17, 0, 47), mes.DateTime);
            Assert.AreEqual("FGHFF GH DFDF", mes.UserName);
            Assert.AreEqual("Hello! My name is '???'", mes.Text);
        }
        [Test]
        public void Parse_UserNameContainsSymbols_UserNameUnchanged()
        {
            string line = "24.08.2011 17:00:47|TIM_!@#$%^> Hello! My name is 'TIM_!@#$%^'";
            MessageDataEntity mes = MessageDataEntity.Parse(line);

            Assert.AreEqual("TIM_!@#$%^", mes.UserName);
        }
        [Test]
        public void Parse_MessageContainsGtSymb_TextUnchanged()
        {
            string line = "24.08.2011 17:00:47|TIM> Hello! My name > is '5cc > 62'";
            MessageDataEntity mes = MessageDataEntity.Parse(line);

            Assert.AreEqual("Hello! My name > is '5cc > 62'", mes.Text);
            Assert.AreEqual("TIM", mes.UserName);
        }
        [Test]
        public void Parse_WithoutSpaceAfterDelSym_TextCorrect()
        {
            string line = "24.08.2011 17:00:47|TIM>Hello! My name > is '5cc > 62'";
            MessageDataEntity mes = MessageDataEntity.Parse(line);

            Assert.AreEqual("Hello! My name > is '5cc > 62'", mes.Text);
            Assert.AreEqual("TIM", mes.UserName);
        }
        [Test]
        public void Parse_CleanLine()
        {
            string line = "24.08.2011 17:00:47|5cc62> Hello! My name is '5cc62'";
            MessageDataEntity mes = MessageDataEntity.Parse(line);

            Assert.AreEqual(new DateTime(2011, 08, 24, 17, 0, 47), mes.DateTime);
            Assert.AreEqual("5CC62", mes.UserName);
            Assert.AreEqual("Hello! My name is '5cc62'", mes.Text);
        }
        [Test]
        public void Parse_HasCleanReply_ReplyNotNull()
        {
            string line = "24.08.2011 17:00:47|NAME1     > 23.08.2011 15:01:02|NAME2     > Hello! My name is '5cc62'";
            MessageDataEntity mes = MessageDataEntity.Parse(line);

            Assert.AreEqual(new DateTime(2011, 08, 24, 17, 0, 47), mes.DateTime);
            Assert.AreEqual("NAME1", mes.UserName);
            Assert.AreEqual("Hello! My name is '5cc62'", mes.Text);
            Assert.NotNull(mes.ReplyToMessageId);
            Assert.AreEqual("23.08.2011 15:01:02|NAME2", mes.ReplyToMessageId);
        }


        [TestCase("t")]
        [TestCase("01.02 11:12:13|test> text", Description ="Year not specified")]
        [TestCase("01.02 11:12:13", Description ="Date format only")]
        [TestCase("01.02 11:12:13|", Description = "Date format with del only")]
        [TestCase("01.02.2020 11:12:13|test", Description ="Without '>' symbol")]
        [TestCase("01.02.2020 11:12:13|test tetet", Description ="Without '>' symbol")]
        [TestCase("99.02.2020 11:12:13|test> text", Description ="Bad day")]
        [TestCase("01.99.2020 11:12:13|test> text", Description = "Bad month")]
        [TestCase("01.99.2020 25:12:13|test> text", Description = "Bad hour")]
        [TestCase("01.99.2020 11:60:13|test> text", Description = "Bad minutes")]
        [TestCase("01.99.2020 11:12:60|test> text", Description = "Bad seconds")]
        [TestCase("01.02. 2020 11:12:13|test> text", Description = "Space in date")]
        [TestCase("01.02.2020 10:11:11|NAME1   >hello\nhello", Description = "Has newline")]        
        public void Parse_BadFormatLine_ThrowFormatException(string line)
        {
            Assert.Throws<FormatException>(() => MessageDataEntity.Parse(line));
        }

        [TestCase("01.02.2020 10:11:11|   > text", Description = "Empty name")]
        [TestCase("01.02.2020 10:11:11|> text", Description = "Empty name")]
        [TestCase("01.02.2020 11:12:13|test>", Description = "Text is null")]
        [TestCase("01.02.2020 11:12:13|test> ", Description = "Text is empty")]
        public void Parse_BadFormatLine_ThrowArgumentNullException(string line)
        {
            Assert.Throws<ArgumentNullException>(() => MessageDataEntity.Parse(line));
        }

        [TestCase("t", null)]
        [TestCase("hello world qwertyu|HELLO", null)]
        [TestCase("01.02.2020|hello", null)]
        [TestCase("01.02.2020 25:11:12|NAME1> Text", null)]
        [TestCase("01.02.2020 -0:11:12|NAME1> Text", null)]
        [TestCase("01.02.2020 textdddd|HELLO", null)]
        [TestCase("01.02.2020 10:11:12 Text", null)]
        [TestCase("01.02.2020 10:11:12| Text", null)]
        [TestCase("01.02.2020 10:11:12| NAME1 Text", null)]
        [TestCase("01.02.2020 10:11:12|NAME1", null)]
        [TestCase("01.02.2020 10:11:12|NAME1> Text", "01.02.2020 10:11:12|NAME1", "Text")]
        [TestCase("01.02.2020 10:11:12|NAME1  >  Text", "01.02.2020 10:11:12|NAME1", "Text")]
        [TestCase("01.02.2020 10:11:12|NAME1>Text", "01.02.2020 10:11:12|NAME1", "Text")]
        [TestCase("01.02.2020 10:11:12|NAME1 Text", "01.02.2020 10:11:12|NAME1", "Text")] 
        [TestCase("01.02.2020 10:11:12|NAME1 Text > Text2", "01.02.2020 10:11:12|NAME1", "Text > Text2")]
        public void Parse_ReplyYearSpecified(string text, string expReplyId, string expText = null)
        {
            string line = "24.08.2011 17:00:47|NAMEORIG> " + text;

            MessageDataEntity mes = MessageDataEntity.Parse(line);

            Assert.AreEqual(expReplyId, mes.ReplyToMessageId);
            Assert.AreEqual(expReplyId == null ? text : expText, mes.Text);
        }

        [TestCase("t", null)]
        [TestCase("hello world qw|HELLO", null)]
        [TestCase("01.02|hello", null)]
        [TestCase("01.02 25:11:12|NAME1> Text", null)]
        [TestCase("01.02 -0:11:12|NAME1> Text", null)]
        [TestCase("01.02 textdddd|HELLO", null)]
        [TestCase("01.02 10:11:12 Text", null)]
        [TestCase("01.02 10:11:12| Text", null)]
        [TestCase("01.02 10:11:12| NAME1 Text", null)]
        [TestCase("01.02 10:11:12|NAME1", null)]
        [TestCase("01.02 10:11:12|NAME1> Text", "01.02.2021 10:11:12|NAME1", "Text")]
        [TestCase("01.02 10:11:12|NAME1  >  Text", "01.02.2021 10:11:12|NAME1", "Text")]
        [TestCase("01.02 10:11:12|NAME1>Text", "01.02.2021 10:11:12|NAME1", "Text")]
        [TestCase("01.02 10:11:12|NAME1 Text", "01.02.2021 10:11:12|NAME1", "Text")]
        [TestCase("01.02 10:11:12|NAME1 Text > Text2", "01.02.2021 10:11:12|NAME1", "Text > Text2")]
        public void Parse_ReplyYearNotSpecified(string text, string expReplyId, string expText = null)
        {
            string line = "24.08.2011 17:00:47|NAMEORIG> " + text;

            MessageDataEntity mes = MessageDataEntity.Parse(line);

            Assert.AreEqual(expReplyId, mes.ReplyToMessageId);
            Assert.AreEqual(expReplyId == null ? text : expText, mes.Text);
        }
    }
}



