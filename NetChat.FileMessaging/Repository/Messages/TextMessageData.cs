using System;

namespace NetChat.FileMessaging.Repository.Messages
{
    public class TextMessageData
    {
        private const string DATE_FORMAT = "dd.MM HH:mm:ss";
        private const int USERNAME_AREA_SIZE = 10;

        public string Id { get; }
        public DateTime DateTime { get; }
        public string UserName { get; }
        public string Text { get; }


        public TextMessageData(string username, string message)
        {
            DateTime = DateTime.Now;
            UserName = username?.ToUpper() ?? throw new ArgumentNullException(nameof(username));
            Text = message.Replace("\n", " ").Replace("  ", " ");
            Id = GetId();
        }

        public TextMessageData(string username, string message, DateTime dateTime)
        {
            DateTime = dateTime;
            UserName = username?.ToUpper() ?? throw new ArgumentNullException(nameof(username));
            Text = message.Replace("\n", " ").Replace("  ", " ");
            Id = GetId();
        }

        public static TextMessageData Parse(string line)
        {
            if (string.IsNullOrEmpty(line))
                throw new ArgumentNullException(line);
            var dateTime = DateTime.ParseExact(line.Substring(0, DATE_FORMAT.Length), DATE_FORMAT, null);
            var userName = line.Substring(DATE_FORMAT.Length + 1, line.IndexOf('>') - DATE_FORMAT.Length - 1).Trim();
            var text = line.Substring(line.IndexOf('>') + 2);
            return new TextMessageData(userName, text, dateTime);
        }

        //public TextMessageData(string line)
        //{
        //    if (string.IsNullOrEmpty(line))
        //        throw new ArgumentNullException(line);
        //    DateTime = DateTime.ParseExact(line.Substring(0, DATE_FORMAT.Length), DATE_FORMAT, null);
        //    UserName = line.Substring(DATE_FORMAT.Length + 1, line.IndexOf('>') - DATE_FORMAT.Length - 1).Trim();
        //    Text = line.Substring(line.IndexOf('>') + 2);
        //    Id = GetId();
        //}

        public override string ToString()
        {
            string res = string.Empty;
            if (Text.Contains("\n"))
            {
                var messages = Text.Split('\n');
                for (int i = 0; i < messages.Length; i++)
                    res += $"{IdToString(Id)}> {Text}" + (i == messages.Length - 1 ? "" : "\n");
            }
            else
                res = $"{IdToString(Id)}> {Text}";
            return res;
        }

        private string GetId()
        {
            return $"{DateTime.ToString(DATE_FORMAT)}|{UserName}";
        }

        private string IdToString(string id)
        {
            var strs = id.Split('|');
            return $"{strs[0]}|{strs[1],-USERNAME_AREA_SIZE}";
        }
    }
}
