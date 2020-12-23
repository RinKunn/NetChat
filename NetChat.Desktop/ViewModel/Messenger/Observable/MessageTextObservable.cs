using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace NetChat.Desktop.ViewModel.Messenger
{
    public class MessageTextObservable : MessageObservable
    {
        public string Text { get; }

        public MessageTextObservable(string text, string id, DateTime dateTime, 
            ParticipantObservable sender, bool isOriginNative = false/*, bool isReaded = true*/)
            : base(id, dateTime, sender, isOriginNative/*, isReaded*/)
        {
            Text = text;
        }
    }
}
