using Newtonsoft.Json;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ClientServerUtil
{
    class Util
    {
        public static byte[] BuildJSON(dynamic content)
        {
            Byte[] JSON = System.Text.Encoding.Unicode.GetBytes(JsonConvert.SerializeObject(content));
            JSON = prependBytes(JSON);
            return JSON;
        }

        public static byte[] prependBytes(Byte[] bytes)
        {
            byte[] lengthPrefix = BitConverter.GetBytes(bytes.Length);

            // Concatenate the length prefix and the message
            byte[] ret = new byte[lengthPrefix.Length + bytes.Length];
            lengthPrefix.CopyTo(ret, 0);
            bytes.CopyTo(ret, lengthPrefix.Length);
            return ret;
        }

        public static String ReadMessage(TcpClient client)
        {
            byte[] buffer = new byte[4];
            int totalRead = 0;
            //read bytes until stream indicates there are no more
            NetworkStream stream = client.GetStream();

            do
            {
                if (!stream.DataAvailable)
                {
                    Thread.Sleep(10);
                    continue;
                }
                int read = stream.Read(buffer, totalRead, buffer.Length - totalRead);
                totalRead += read;
            } while (totalRead != 4);

            buffer = new byte[BitConverter.ToUInt32(buffer, 0)];
            totalRead = 0;
            do
            {
                if (!stream.DataAvailable)
                {
                    Thread.Sleep(10);
                    continue;
                }
                int read = stream.Read(buffer, totalRead, buffer.Length - totalRead);
                totalRead += read;
            } while (totalRead != buffer.Length);

            return Encoding.Unicode.GetString(buffer, 0, totalRead);
        }
    }
}