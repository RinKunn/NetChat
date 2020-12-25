using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat.Desktop.Repository
{
    public class UserData
    {
        public string UserId { get; set; }
        public bool IsOnline { get; set; }
        public DateTime LastChanged { get; set; }
    }
}
