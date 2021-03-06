﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NetChat.FileMessaging.Common;
using NetChat.FileMessaging.Repository.Messages;

namespace NetChat.FileMessaging.Repository.UserStatuses
{
    public class UserStatusFileRepository : IUserStatusRepository
    {
        private readonly string _filename;
        private readonly Encoding _encoding;

        public UserStatusFileRepository(RepositoriesConfig config)
        {
            _filename = config.MessagesSourcePath;
            _encoding = config.MessagesSourceEncoding;
        }

        public async Task<IList<UserStatus>> GetUsersStatuses(CancellationToken token = default)
        {
            var res = await Task.Run(() =>
            {
                var lines = File.ReadAllLines(_filename, _encoding);
                return lines
                    .Select(l => MessageDataEntity.Parse(l))
                    .GroupBy(m => m.UserName)
                    .Select(g =>
                    {
                        var mes = g.Last();
                        return new UserStatus()
                        {
                            UserId = mes.UserName,
                            IsOnline = mes.Text != "Logout",
                            UpdateDateTime = mes.DateTime
                        };
                    })
                    .OrderBy(u => u.UserId)
                    .ToArray();
            }, token);
            return res;
        }

        //public async Task<IList<UserStatus>> GetUsersStatuses(CancellationToken token = default)
        //{
        //    var lines = await FileHelper.GetStringMessagesAsync(_filename, _encoding, 0, token);
        //    return lines
        //            .Select(l => TextMessageData.Parse(l))
        //            .GroupBy(m => m.UserName)
        //            .Select(g =>
        //            {
        //                var mes = g.Last();
        //                return new UserStatus()
        //                {
        //                    UserId = mes.UserName,
        //                    IsOnline = mes.Text != "Logout",
        //                    UpdateDateTime = mes.DateTime
        //                };
        //            })
        //            .OrderBy(u => u.UserId)
        //            .ToArray();
        //}

        public async Task SetUserStatus(UserStatus userStatus)
        {
            await Task.Run(() => 
                File.AppendAllText(_filename,
                    new MessageDataEntity(userStatus.UserId,
                        userStatus.IsOnline ? "Logon" : "Logout",
                        userStatus.UpdateDateTime)
                    .ToString() + '\n',
                    _encoding));
        }
    }
}
