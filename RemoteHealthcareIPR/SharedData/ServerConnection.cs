using ClientServerUtil;
using Newtonsoft.Json;
using SharedData.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DoctorApp
{
    public delegate void ReceiveResponse(SessionSnapshot jsonResponse);

    class ServerConnection
    {
        private static TcpClient Client;
        Thread ReadThread;
        Thread SendThread;
        NetworkStream stream;
        public event ReceiveResponse OnReceiveResponse;


        public ServerConnection() { 
            byte[] data = new byte[1024];
            Client = new TcpClient(IPAddress.Loopback.ToString(), 667);
            stream = Client.GetStream();
            //byte[] messageBytes = Util.BuildJSON(data);
            //stream.Write(messageBytes, 0, messageBytes.Length);
        
        
            ReadThread = new Thread(RecieveServerData);
            ReadThread.Start();

        }

        private void SendData(SessionSnapshot snapshot)
        {
            byte[] messageBytes = Ut
        }

        private void RecieveServerData()
        {
            while (true)
            {
                SessionSnapshot receivedData = JsonConvert.DeserializeObject<SessionSnapshot>(Util.ReadMessage(Client));
            }
        }

        private void ResponseHandler(SessionSnapshot jsonResponse)
        {
            Task.Run(() => OnReceiveResponse?.Invoke(jsonResponse));
        }
    }
}
