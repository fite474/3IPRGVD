using ClientServerUtil;
using Newtonsoft.Json;
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

namespace DoctorApp
{
    public delegate void ReceiveResponse(Datagram jsonResponse);

    class ServerConnection
    {
        private static TcpClient Client;
        Thread ReadThread;
        Thread SendThread;
        NetworkStream stream;
        public event ReceiveResponse OnReceiveResponse;


        public ServerConnection(bool doc) { 
            byte[] data = new byte[1024];
            Client = new TcpClient(IPAddress.Loopback.ToString(), 667);
            stream = Client.GetStream();
            Datagram datagram = new Datagram();
            if (doc)
            {
                datagram.DataType = DataType.ImDoc;
            }
            else
            {
                datagram.DataType = DataType.ImClient;
            }
            byte[] messageBytes = Util.BuildJSON(datagram);
            stream.Write(messageBytes, 0, messageBytes.Length);
        
        
            ReadThread = new Thread(RecieveServerData);
            ReadThread.Start();

        }

        public void SendData(Datagram snapshot)
        {
            byte[] messageBytes = Util.BuildJSON(snapshot);
            stream.Write(messageBytes, 0, messageBytes.Length);
        }

        private void RecieveServerData()
        {
            while (true)
            {
                Datagram receivedData = JsonConvert.DeserializeObject<Datagram>(Util.ReadMessage(Client));
            }
        }
            
        private void ResponseHandler(Datagram jsonResponse)
        {
            Task.Run(() => OnReceiveResponse?.Invoke(jsonResponse));
        }
    }
}
