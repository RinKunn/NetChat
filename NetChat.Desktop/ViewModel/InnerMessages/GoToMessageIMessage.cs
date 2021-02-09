using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat.Desktop.ViewModel.InnerMessages
{
    public class GoToMessageIMessage : IInnerMessage
    {
        public string Id { get; }

        public GoToMessageIMessage(string id)
        {
            Id = id;
        }
    }
}
