using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat.Desktop.Repository
{
    public interface IUserRepository
    {
        Task<IList<UserData>> GetUsers();
        Task<UserData> GetUserById(string userId);
    }
}
