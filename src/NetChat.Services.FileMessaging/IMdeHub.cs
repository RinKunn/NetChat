using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetChat.Services.FileMessaging
{
    public interface IMdeHub : IDisposable
    {
        event OnUpdateEventHandler OnUpdate;
        bool IsConnected { get; }
        void Connect();
        void Disconnect();
    }

    public delegate Task OnUpdateEventHandler(OnUpdateEventArgs args);

    public class OnUpdateEventArgs
    {
        public IList<MessageDataEntity> Result { get; }
        public OnUpdateEventArgs(IList<MessageDataEntity> entities)
        {
            Result = entities;
        }
    }
}
