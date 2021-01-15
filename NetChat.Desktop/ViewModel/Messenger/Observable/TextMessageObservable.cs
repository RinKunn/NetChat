using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace NetChat.Desktop.ViewModel.Messenger
{
    public class TextMessageObservable : MessageObservable
    {
        public string Text { get; }

        public TextMessageObservable(string text, string id, DateTime dateTime, 
            ParticipantObservable sender, bool isOriginNative = false)
            : base(id, dateTime, sender, isOriginNative)
        {
            Text = text;
        }
    }
}
