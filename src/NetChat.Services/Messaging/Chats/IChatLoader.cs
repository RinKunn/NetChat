using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat.Services.Messaging.Chats
{
    public interface IChatLoader
    {
        ChatData GetChatData();
        void SetChatData(ChatData chatData);
    }
}
