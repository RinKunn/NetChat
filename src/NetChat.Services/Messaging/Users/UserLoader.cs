//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.Extensions.Caching.Memory;
//using NetChat.Services.Caching;
//using NetChat.Services.FileMessaging;
//using NetChat.Services.Messaging.Users.Storage;
//using System.Runtime.Caching;
//using NetChat.Services.Messaging.Messages.Storage;

//namespace NetChat.Services.Messaging.Users
//{
//    public class UserLoader : IUserLoader
//    {
//        private readonly IDataCache _cache;
//        private readonly IMessageDataRepository _messageDataRepository;
//        private readonly IUserDataRepository _userDataRepository;

//        public async Task<UserData> GetMeAsync()
//        {
//            var policy = new CacheItemPolicy()
//            {
//                SlidingExpiration = TimeSpan.FromSeconds(5)
//            };
//            return await _cache.GetOrCreate<UserData>(CacheKeys.ME, GetMyUserDataAsync, policy);
//        }

//        public async Task<int> GetOnlineUsersCount()
//        {
            
//        }


//        private async Task<UserData> GetMyUserDataAsync()
//        {
//            string currentUserName =
//#if DEBUG
//            Environment.UserName.ToUpper();
//            string currentProcess = Process.GetCurrentProcess().ProcessName;
//            int runnedProc = Process.GetProcessesByName(currentProcess).Length;
//            if (runnedProc > 1)
//                currentUserName += runnedProc.ToString();
//#else
//            Environment.UserName.ToUpper();
//#endif
//            return await _userDataRepository
//                    .GetByUserName(currentUserName);
//        }
//    }
//}
