using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace NetChat.FileMessaging.Repository.Messages
{
    public class MessageDataEntity
    {
        private const string IDFORMAT = @"(\d{2}.\d{2}(.\d{4})? \d{2}:\d{2}:\d{2})\|(\S*)";
        private const string DATE_FORMAT = "dd.MM.yyyy HH:mm:ss";
        private const string DATE_FORMAT_ALT = "dd.MM HH:mm:ss";
        private const int USERNAME_AREA_SIZE = 10;
        
        public string Id { get; }
        public DateTime DateTime { get; }
        public string UserName { get; }
        public string Text { get; }
        public string ReplyToMessageId { get; }


        public MessageDataEntity(string username, string message)
         : this(username, message, DateTime.Now, null) { }

        public MessageDataEntity(string username, string message, DateTime dateTime)
         : this(username, message, dateTime, null) { }

        public MessageDataEntity(string username, string message, 
            DateTime dateTime, string replyToMessageId)
        {
            if (string.IsNullOrEmpty(username))
                throw new ArgumentNullException(nameof(username));
            if (string.IsNullOrEmpty(message))
                throw new ArgumentNullException(nameof(message));
            if(message.Contains("\n"))
                throw new FormatException($"{this.GetType().Name} should't have newline symbol");
            if(!string.IsNullOrEmpty(replyToMessageId)
                && !IsIdMatchToFormat(replyToMessageId))
                throw new FormatException($"Reply message id ('{replyToMessageId}') has wrong format");

            DateTime = dateTime;
            UserName = username.ToUpper();
            Text = message;
            Id = CreateId(UserName, DateTime);
            ReplyToMessageId = string.IsNullOrEmpty(replyToMessageId) ? null : replyToMessageId;
        }
        
        private bool IsIdMatchToFormat(string input)
        {
            Regex regex = new Regex(IDFORMAT);
            var match = regex.Match(input);
            if (!match.Success
                || (
                    !DateTime.TryParseExact(match.Groups[1].Value, DATE_FORMAT_ALT, null, DateTimeStyles.None, out _)
                    && !DateTime.TryParseExact(match.Groups[1].Value, DATE_FORMAT, null, DateTimeStyles.None, out _))
                || string.IsNullOrEmpty(match.Groups[3].Value)
                || !IsLettersUpper(match.Groups[3].Value))
                return false;
            Console.WriteLine("{0} -- ", match.Groups[3].Value);
            return true;
        }
        

        public static MessageDataEntity ParseOrDefault(string line)
        {
            MessageDataEntity res = null;
            try
            {
                res = Parse(line);
            }
            catch(Exception ex)
            {
                res = null;
            }
            return res;
        }

        public static MessageDataEntity Parse(string line)
        {
            if (string.IsNullOrEmpty(line))
                throw new ArgumentNullException($"Parsing line is empty");

            if (line.Length <= DATE_FORMAT.Length)
                throw new FormatException($"Line length ({line.Length}) is very short: '{line}'");
            
            if(!DateTime.TryParseExact(
                line.Substring(0, DATE_FORMAT.Length),
                DATE_FORMAT, 
                null, DateTimeStyles.None,
                out DateTime dateTime))
                throw new FormatException($"Line's MessageId dateTime part has wrong format: '{line}'");

            int idDelimiterIndex = line.IndexOf('>');
            if (idDelimiterIndex == -1)
                throw new FormatException($"Line has not MessageId delimeter: '{line}'");

            var userName = line.Substring(DATE_FORMAT.Length + 1, idDelimiterIndex - DATE_FORMAT.Length - 1).Trim();
            var text = line.Substring(line.IndexOf('>') + 1).Trim();
            string replyId = null;

            if((text.Length > DATE_FORMAT.Length && text[DATE_FORMAT.Length] == '|')
                || (text.Length > DATE_FORMAT_ALT.Length && text[DATE_FORMAT_ALT.Length] == '|'))
            {
                replyId = ExtractReplyId(text, DATE_FORMAT, out int replyIdLastIndex);
                if(replyId == null)
                    replyId = ExtractReplyId(text, DATE_FORMAT_ALT, out replyIdLastIndex);

                if (replyId != null)
                {
                    if (replyIdLastIndex < text.Length - 1)
                        text = text.Substring(replyIdLastIndex + 1).Trim();
                    else
                        text = null;
                }
            }

            return new MessageDataEntity(userName, text, dateTime, replyId);
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = 
                new StringBuilder(IdToString(Id) + "> ");
            if (!string.IsNullOrEmpty(ReplyToMessageId))
                stringBuilder.Append(IdToString(ReplyToMessageId) + "> ");
            stringBuilder.Append(Text);
            return stringBuilder.ToString();
        }

        private static string CreateId(string userName, DateTime dateTime)
        {
            return $"{dateTime.ToString(DATE_FORMAT)}|{userName}";
        }

        private static string IdToString(string id)
        {
            var strs = id.Split('|');
            return $"{strs[0]}|{strs[1],-USERNAME_AREA_SIZE}";
        }


        private bool IsLettersUpper(string input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                if (char.IsLetter(input[i]) && !char.IsUpper(input[i]))
                    return false;
            }
            return true;
        }

        private static bool IsLettersUpperUntilChar(string input, char del)
        {
            bool hasLetter = false, hasDelim = false;
            int i = 0;
            if (!char.IsLetter(input[0]))
                return false;
            for (i = 0; i < input.Length; i++)
            {
                if (char.IsLetter(input[i]))
                {
                    hasLetter = true;
                    if(!char.IsUpper(input[i]))
                        return false;
                }
                else if(input[i] == del)
                {
                    hasDelim = true;
                    break;
                }
            }
            return hasDelim && hasLetter;
        }

        private static string ExtractReplyId(string text, string dateFormat, out int lastReplyIdIndex)
        {
            lastReplyIdIndex = -1;
            if (text.Length <= dateFormat.Length + 1)
                return null;
            if (!DateTime.TryParseExact(
                    text.Substring(0, dateFormat.Length),
                    dateFormat, null,
                    DateTimeStyles.None, out DateTime dateTime)
                || text[dateFormat.Length] != '|')
                return null;

            string textWithoutDate = text.Substring(dateFormat.Length + 1);

            if (IsLettersUpperUntilChar(textWithoutDate, '>'))
            {
                var charindex = textWithoutDate.IndexOf('>');
                lastReplyIdIndex = charindex + dateFormat.Length + 1;
                return CreateId(textWithoutDate.Substring(0, charindex).Trim(),
                        dateTime);
            }

            if (IsLettersUpperUntilChar(textWithoutDate, ' '))
            {
                var charindex = textWithoutDate.IndexOf(' ');
                lastReplyIdIndex = charindex + dateFormat.Length + 1;
                return CreateId(textWithoutDate.Substring(0, charindex).Trim(),
                    dateTime);
            }

            return null;
        }
    }
}
