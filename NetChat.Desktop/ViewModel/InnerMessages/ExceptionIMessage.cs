﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat.Desktop.ViewModel.InnerMessages
{
    public class ExceptionIMessage : IInnerMessage
    {
        public string ErrorMessage { get; private set; }

        public ExceptionIMessage(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}
