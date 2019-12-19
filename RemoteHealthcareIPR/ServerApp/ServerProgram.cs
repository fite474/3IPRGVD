using ClientServerUtil;
using Newtonsoft.Json;
using ServerApp.Tasks;
using SharedData.Data;
using SharedData.Json;
using System;
using System.Collections.Generic;
using System.IO;
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

        public static void VerifyDir(string path)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(path);
                if (!dir.Exists)
                {
                    dir.Create();
                }
            }
            catch { }
        }

        public static void Logger(dynamic receivedData)
        {
            string lines = JsonConvert.SerializeObject(receivedData, Formatting.Indented);

            //dynamic array = JsonConvert.DeserializeObject<List<Session>>(lines);



            string path = $"ClientDataLogs/";//"C:/IPR_LOG/";
            VerifyDir(path);
            string fileName = "Astrand_Test_" +
                DateTime.Now.Day.ToString() + "_" +
                DateTime.Now.Month.ToString() + "_" +
                DateTime.Now.Year.ToString() + "_" +
                DateTime.Now.Hour.ToString() + "_" +
                DateTime.Now.Minute.ToString() + "_" +
                DateTime.Now.Second.ToString() + 
                "_Logs.txt";
            try
            {
                System.IO.StreamWriter file = new System.IO.StreamWriter(path + fileName, true);
                file.WriteLine(DateTime.Now.ToString() + ": " + lines);

                //using (var writer = new StreamWriter(path + fileName))
                //{
                //    writer.Write(lines);

                //}


                file.Close();
            }
            catch (Exception) { }
        }

        private void ReceiveServerData(TcpClient tcp)
        {
            while (true)
            {
                Datagram receivedData = JsonConvert.DeserializeObject<Datagram>(Util.ReadMessage(tcp));
                Console.WriteLine(receivedData.DataType);

                switch (receivedData.DataType)
                {
                    case DataType.StartSession:
                        {
                            SendDataToClient(receivedData);
                            break;
                        }
                    case DataType.SessionSnapshot:
                        {
                            SendDataToDoc(receivedData);
                            break;
                        }
                    case DataType.ImClient:
                        {
                            client = tcp;
                            break;
                        }
                    case DataType.ImDoc:
                        {
                            doctor = tcp;
                            break;
                        }
                    case DataType.LogData:
                        {
                            Logger(receivedData);
                            break;
                        }
                }
            }
        }

        public void SendDataToDoc(Datagram snapshot)
        {
            byte[] messageBytes = Util.BuildJSON(snapshot);
            doctor.GetStream().Write(messageBytes, 0, messageBytes.Length);
        }


        public void SendDataToClient(Datagram snapshot)
        {
            byte[] messageBytes = Util.BuildJSON(snapshot);
            client.GetStream().Write(messageBytes, 0, messageBytes.Length);
        }

    }
}
