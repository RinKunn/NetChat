using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat.Desktop.Repository
{
    public abstract class MessageData
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public DateTime DateTime { get; set; }
    }
}
