using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NetChat.FileMessaging.Repository.UserStatuses
{
    public interface IUserStatusRepository
    {
        Task<IList<UserStatus>> GetUsersStatuses(CancellationToken token = default);
        Task SetUserStatus(UserStatus userStatus);
    }
}
