using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat.Services.Messaging.Messages.Storage
{
    public interface IMdeHub
    {
        event OnUpdateHandler OnUpdate;
    }

    public delegate void OnUpdateHandler(List<MessageDataEntity> newMessages);
}
