using ClientServerUtil;
using Newtonsoft.Json;
using ServerApp.Tasks;
using SharedData.Data;
using SharedData.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerApp
{
    class ServerProgram
    {
        bool doc = true;
        TcpClient client;
        TcpClient doctor;
        public static void Main(string[] args)
        {
            new ServerProgram();
        }

        private TcpListener server;
        private AutoResetEvent allDone = new AutoResetEvent(false);
        private readonly List<TcpTask> currentUsers = new List<TcpTask>();
        private ServerData serverData;

        public ServerProgram()
        {


            serverData = ServerData.LoadData();
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            try
            {
                server = new TcpListener(IPAddress.Loopback, 667);
                server.Start();
                while (true)
                {
                    TcpClient tcpClient = server.AcceptTcpClient();
                    Task receiveServerData = new Task(() => ReceiveServerData(tcpClient));
                    receiveServerData.Start();
                    //server.BeginAcceptTcpClient(new AsyncCallback(OnConnect), null);
                    //allDone.WaitOne();
                    //Console.WriteLine("Client has connected");
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
        }

        private void OnConnect(IAsyncResult ar)
        {
                //doctor = server.EndAcceptTcpClient(ar);
                //TypeCheckTask check = new TypeCheckTask(doctor, currentUsers, serverData);
                //allDone.Set();
        }

        private void ReceiveServerData(TcpClient tcp)
        {
            while (true)
            {
                Console.WriteLine("??????????????????????????????????????????????????????");
                Datagram receivedData = JsonConvert.DeserializeObject<Datagram>(Util.ReadMessage(tcp));
                Console.WriteLine(receivedData.DataType);
            }
        }
    }
}
