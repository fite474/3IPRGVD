using PatientApp.Bike;
using SharedData.Data;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.Bike
{
    public class BikeConnection
    {
        private BikeTask bikeTask = new BikeTask();
        private long loopInterval = 1000; //In milliseconds

        public BikeConnection()
        {
        }

        public void FindBike()
        {
            bool portInUse = false;

            if (SerialPort.GetPortNames().Length > 0)
                portInUse = true;

            bikeTask = new BikeTask();
            if (!portInUse)
                bikeTask.UseSimulator();
            else
            {
                bool bikeAvailable = false;

                using (var searcher = new ManagementObjectSearcher("SELECT * FROM WIN32_SerialPort"))
                {
                    string[] portnames = SerialPort.GetPortNames();
                    var ports = searcher.Get().Cast<ManagementBaseObject>().ToList();

                    var tList = (from n in portnames
                                 join p in ports on n equals p["DeviceID"].ToString()
                                 select n + "-" + p["Caption"]).ToList();

                    foreach (string caption in tList)
                    {
                        if (caption.Contains("Silicon Labs CP210x USB to UART Bridge"))
                        {
                            string[] splitted = caption.Split('-');
                            bikeTask.ChangeBike(splitted[0]);
                            bikeAvailable = true;
                            break;
                        }
                    }
                }
                if (!bikeAvailable)
                    bikeTask.UseSimulator();
            }
        }

        public void RunBikeLoop(object o)
        {
            SessionSnapshot currentBikeData;
            bool isSessionRunning = true;

            long startTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            while (isSessionRunning)
            {
                long currentTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

                if (currentTime - startTime >= loopInterval)
                {
                    startTime = currentTime;

                    currentBikeData = bikeTask.GetBikeData();
                    SessionSnapshot snapshot = new SessionSnapshot
                    {
                        Speed = currentBikeData.Speed,
                        Distance = currentBikeData.Distance,
                        Energy = currentBikeData.Energy,
                        HeartRate = currentBikeData.HeartRate,
                        CurrentPower = currentBikeData.CurrentPower,
                        DateTime = DateTime.Now,
                        Power = currentBikeData.Power,
                        Rpm = currentBikeData.Rpm
                    };

                    Console.WriteLine(snapshot);

                }
            }

        }
    }
}
