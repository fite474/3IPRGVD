using SharedData.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp.Tasks
{
    public abstract class TcpTask
    {
        public TcpClient Client { get; }

        public IUserData UserData { get; }

        public abstract void Run();

        public TcpTask(TcpClient client, IUserData userData)
        {
            this.UserData = userData;
            this.Client = client;
        }
    }
}
