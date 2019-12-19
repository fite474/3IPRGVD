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
        private string oldStringData;
        private SerialPort comPort;
        public BikeKettler(string port)
        {
            comPort = new SerialPort(port);
            comPort.Handshake = Handshake.XOnXOff;
            comPort.Open();
            
            comPort.WriteLine("rs");
            comPort.WriteLine("cm");

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
            Console.WriteLine("---------------------------------"+stringData);
            if (stringData.Contains("ERROR"))
                return null;
            if (stringData.Contains("ACK"))
            {
                if (oldStringData != null)
                {
                    //return BikeDataHelper.AssignValues(oldStringData, 0);
                    //comPort.WriteLine("st");
                    comPort.ReadLine();//return ReadData();// return BikeDataHelper.AssignValues(oldStringData, 0); 
                    stringData = comPort.ReadLine();
                    Console.WriteLine("recalculated" + stringData);
                    //Console.WriteLine("retry");
                }
                else
                {
                    Console.WriteLine("null");
                    return null;
                }
            }

            if (stringData.Contains("RUN"))
            {
                if (oldStringData != null)
                {
                    //return BikeDataHelper.AssignValues(oldStringData, 0);
                    //comPort.WriteLine("st");
                    comPort.ReadLine();//return ReadData();// return BikeDataHelper.AssignValues(oldStringData, 0); 
                    stringData = comPort.ReadLine();
                    Console.WriteLine("recalculated" + stringData); //return ReadData();//return BikeDataHelper.AssignValues(oldStringData, 0);
                    
                    //Console.WriteLine("retry");
                }
                else
                {
                    Console.WriteLine("null");
                    return null;
                }
            }
            Console.WriteLine(DateTime.Now.Second);
            oldStringData = stringData;
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
            comPort.WriteLine("cm");
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
