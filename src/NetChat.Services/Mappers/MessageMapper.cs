//using System.Threading.Tasks;
//using NetChat.Services.Caching;
//using NetChat.Services.Messaging.Messages;
//using NetChat.Services.Messaging.Messages.Storage;
//using NetChat.Services.Messaging.Users;

//namespace NetChat.Services.Mappers
//{
//    internal class MessageMapper : IMessageMapper
//    {
//        private readonly IDataCache _cache;

//        public MessageMapper(IDataCache cache)
//        {
//            _cache = cache;
//        }

//        public Task<Message> ToMessageAsync(MessageDataEntity mde)
//        {
//            if (!IsMessage(mde))
//                return null;

//        }

//        private bool IsMessage(MessageDataEntity mde)
//        {
//            return mde.Text != null
//                && mde.Text != "Logon"
//                && mde.Text != "Logout";
//        }

//        private MessageData ToMessageData(MessageDataEntity mde)
//        {
//            return new MessageData(
//                mde.Id,
//                mde.UserName,
//                mde.DateTime,
//                mde.UserName == _cache.GetItem<string>(CacheKeys.CURRENTUSERNAME),
//                new MessageTextContent(mde.Text),
//                mde.ReplyToMessageId);
//        }

//        public UserStatus ToUserStatus(MessageDataEntity mde)
//        {
//            return new UserStatus(
//                mde.UserName,
//                mde.Text == "Logout" ? false : true,
//                mde.DateTime);
//        }
//    }
//}
