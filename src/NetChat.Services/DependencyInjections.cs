using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace NetChat.Services
{
    public static class DependencyInjections
    {
        public static ContainerBuilder RegisterAppServices(this ContainerBuilder builder)
        {
            return builder;
        }
    }
}
