using SharedData.Data;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientApp.Bike
{
    class BikeKettler : IBike
    {
        private SerialPort comPort;
        public BikeKettler(string port)
        {
            comPort = new SerialPort(port);
            comPort.Handshake = Handshake.XOnXOff;
            comPort.Open();

        }

        public void Change(int hr, int spd, int en)
        {

        }

        public void Close()
        {
            comPort.Close();
        }

        public SessionSnapshot ReadData()
        {
            comPort.WriteLine("st");
            string stringData = comPort.ReadLine();
            Console.WriteLine(stringData);
            if (stringData.Contains("ERROR"))
                return null;

            return BikeDataHelper.AssignValues(stringData, 0);

        }

        public void PutDistance(int distance)
        {
            comPort.WriteLine("rs");
            comPort.WriteLine("cd");
            comPort.WriteLine("pd " + distance);
        }

        public void PutPower(int power)
        {
            //comPort.WriteLine("rs");
            //comPort.WriteLine("cd");
            comPort.WriteLine("pw " + power);
        }

        public void PutTime(int time)
        {
            comPort.WriteLine("rs");
            comPort.WriteLine("cd");
            comPort.WriteLine("pt " + time);
        }
    }


}
