using System.Collections.Generic;
using System.Threading.Tasks;
using NetChat.Desktop.ViewModel.Messenger;

namespace NetChat.Desktop.Services.Messaging.Users
{
    public interface IUserLoader
    {
        Task<IList<ParticipantObservable>> GetUsers();
        Task<ParticipantObservable> GetUserById(string userId);
        bool IsMe(string userId);
        Task<int> OnlineUsersCount();
    }
}
