using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat.Desktop.ViewModel.InnerMessages
{
    public class ExceptionInnerMessage : IInnerMessage
    {
        public string ErrorMessage { get; private set; }

        public ExceptionInnerMessage(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}
