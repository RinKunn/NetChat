using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat.Desktop.ViewModel.InnerMessages
{
    public class ParticipantLoggedInIMessage : IInnerMessage
    {
        public ParticipantLoggedInIMessage(string userId, DateTime updateDateTime)
        {
            UserId = userId;
            UpdateDateTime = updateDateTime;
        }

        public string UserId { get; }
        public DateTime UpdateDateTime { get; }

    }
}
