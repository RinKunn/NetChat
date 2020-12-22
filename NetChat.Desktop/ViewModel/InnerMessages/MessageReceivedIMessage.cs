using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat.Desktop.ViewModel.InnerMessages
{
    internal class MessageReceivedIMessage : IInnerMessage
    {
        public MessageReceivedIMessage(string id, string userId, DateTime dateTime, string text)
        {
            Id = id;
            UserId = userId;
            DateTime = dateTime;
            Text = text;
        }

        public string Id { get; }
        public string UserId { get; }
        public DateTime DateTime { get; }
        public string Text { get; }

    }
}
