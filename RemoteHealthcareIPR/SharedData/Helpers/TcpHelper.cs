using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SharedData.Cryptography;
using SharedData.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace SharedData.Helpers
{
    class TcpHelper
    {
        TcpClient client;
        private int receiveLength;
        private int totalLength;
        private byte[] buffer = new byte[1024];
        Action<Datagram> onCompletedEvent;
        private string totalBuffer = "";

        public TcpHelper(TcpClient client)
        {
            this.client = client;
        }
        public static T ToConcreteType<T>(dynamic type)
        {
            return (type as JObject).ToObject<T>();
        }

        public void ReadJsonData(Action<Datagram> onCompletedEvent)
        {
            Console.WriteLine("8" + onCompletedEvent);
            this.onCompletedEvent = onCompletedEvent;
            byte[] lengthBuffer = new byte[4];
            client.GetStream().BeginRead(lengthBuffer, 0, lengthBuffer.Length, OnReadLengthPrefix, lengthBuffer);
        }

        public void WriteJsonData(Datagram jsonData)
        {
            Console.WriteLine("4" + jsonData);
            string jsonString = JsonConvert.SerializeObject(jsonData);

            string encryptedString = Crypto.Encrypt(jsonString);
            Console.WriteLine("Encrypted(2) : " + encryptedString);
            byte[] jsonStringByteData = Encoding.UTF8.GetBytes(encryptedString);
            byte[] messageLength = BitConverter.GetBytes(jsonStringByteData.Length);
            client.GetStream().BeginWrite(messageLength, 0, messageLength.Length, EndWriteLength, jsonStringByteData);

        }


        private void EndWriteLength(IAsyncResult asyncResult)
        {
            byte[] jsonStringByteData = (byte[])asyncResult.AsyncState;
            client.GetStream().EndWrite(asyncResult);
            client.GetStream().BeginWrite(jsonStringByteData, 0, jsonStringByteData.Length, null, null);

        }


        private void OnReadLengthPrefix(IAsyncResult lengthResult)
        {
            byte[] lengthBuffer = (byte[])lengthResult.AsyncState;
            receiveLength = BitConverter.ToInt32(lengthBuffer, 0);


            client.GetStream().BeginRead(buffer, 0, buffer.Length, OnReadData, null);
        }

        private void OnReadData(IAsyncResult dataResult)
        {
            int bytesRead = client.GetStream().EndRead(dataResult);
            if (totalLength < receiveLength)
            {
                string decodedBuffer = Encoding.UTF8.GetString(buffer.Take(bytesRead).ToArray());
                totalBuffer += decodedBuffer.Substring(0, decodedBuffer.Length);
                totalLength += bytesRead;
                buffer = new byte[1024];
                if (totalLength < receiveLength)
                {
                    client.GetStream().BeginRead(buffer, 0, buffer.Length, OnReadData, null);
                }
                else
                {
                    Console.WriteLine("Encrypted: " + totalBuffer);

                    ParseToJSON(totalBuffer);
                }
            }

        }


        private void ParseToJSON(string data)
        {
            String decryptedData = Crypto.Decrypt(data);

            Console.WriteLine("Decrypted: " + decryptedData);
            dynamic jsonData = JsonConvert.DeserializeObject<Datagram>(data);
            onCompletedEvent(jsonData);
        }
    }
}
