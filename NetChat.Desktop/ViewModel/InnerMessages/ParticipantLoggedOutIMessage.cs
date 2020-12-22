using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat.Desktop.ViewModel.InnerMessages
{
    public class ParticipantLoggedOutIMessage : IInnerMessage
    {
        public ParticipantLoggedOutIMessage(string userId, DateTime updateDateTime)
        {
            UserId = userId;
            UpdateDateTime = updateDateTime;
        }

        public string UserId { get; }
        public DateTime UpdateDateTime { get; }
    }
}
