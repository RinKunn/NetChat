using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat.Desktop.ViewModel.InnerMessages
{
    public class ParticipantStatusChangedIMessage : IInnerMessage
    {
        public ParticipantStatusChangedIMessage(string userId, bool isLoggedIn, DateTime updateDateTime)
        {
            UserId = userId;
            IsLoggedIn = isLoggedIn;
            UpdateDateTime = updateDateTime;
        }

        public bool IsLoggedIn { get; }
        public string UserId { get; }
        public DateTime UpdateDateTime { get; }

    }
}
