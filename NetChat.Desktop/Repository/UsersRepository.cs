using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat.Desktop.Repository
{
    public class UsersRepository : IUserRepository
    {
        private readonly string _filePath;
        private readonly Encoding _encoding;

        public UsersRepository(RepositoryConfigs repositoryConfigs)
        {
            _filePath = repositoryConfigs.MessagesSourcePath;
            _encoding = repositoryConfigs.MessagesSourceEncoding;
        }

        public async Task<UserData> GetUserById(string userId)
        {
            return (await GetUsers())
                .FirstOrDefault(u => u.UserId == userId);
        }

        public async Task<IList<UserData>> GetUsers()
        {
            var lines = await FileHelper.GetStringMessagesAsync(_filePath, _encoding);
            return lines
                .Select(l => new NetChatMessage(l))
                .GroupBy(m => m.UserName)
                .Select(g => g.Last())
                .Select(m => new UserData()
                {
                     UserId = m.UserName,
                     IsOnline = m.Text != "Logout",
                     LastChanged = m.DateTime
                })
                .ToArray();
        }
    }
}
