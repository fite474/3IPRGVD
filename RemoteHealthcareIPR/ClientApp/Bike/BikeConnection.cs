using PatientApp.Bike;
using PatientApp.Gui;
using SharedData.Data;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PatientApp.Bike
{
    public class BikeConnection
    {
        private BikeTask bikeTask = new BikeTask();
        private long loopInterval = 1000; //In milliseconds
        private int timeMinutes = 7;
        private int timeSeconds = 0;
        //private Session Session { get; }
        private readonly int speed = 100;
        public string Age { get; set; }
        public string Weight { get; set; }
        public string Gender { get; set; }

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
            

            while (timeMinutes >= 5)
            {
                CycleRun(RandomNumber(60, 80), 40, ConvertTimeToString(timeMinutes, timeSeconds));
            }
            while (timeMinutes >= 1)
            {
                CycleRun(RandomNumber(80, 140), 120, ConvertTimeToString(timeMinutes, timeSeconds));
            }
            while (timeMinutes >= 0)
            {
                CycleRun(RandomNumber(60, 120), 60, ConvertTimeToString(timeMinutes, timeSeconds));
            }
        }

        public void CycleRun(int heartbeat, int currentPower, string time)
        {
            bool secondsIsZero = timeSeconds == 0;
            Console.WriteLine(Age);
            Console.WriteLine(Weight);
            Console.WriteLine("Heartbeat: " + heartbeat + ", currentPower: " + currentPower + ", Time: " + time);
            if (timeMinutes < 2)
            {
                if (timeSeconds % 15 == 0)
                {
                    SessionSnapshot ss = new SessionSnapshot();
                    ss.CurrentPower = currentPower;
                    ss.Time = time;
                    ss.HeartRate = heartbeat;
                    ss.Rpm = 40;
                    ss.Speed = 30;
                    ss.Distance = 31;
                    ss.Energy = 900;
                    //Session.SessionSnapshots.Add(ss);
                }
            }
            else
            {
                if (timeSeconds == 0)
                {
                    SessionSnapshot ss = new SessionSnapshot();
                    ss.CurrentPower = currentPower;
                    ss.Time = time;
                    ss.HeartRate = heartbeat;
                    ss.Rpm = 40;
                    ss.Speed = 30;
                    ss.Distance = 31;
                    ss.Energy = 900;
                    //Session.SessionSnapshots.Add(ss);
                }
            }

            if (timeSeconds == 0)
            {
                timeSeconds = 59;
                timeMinutes--;
            }
            else timeSeconds--;

            Thread.Sleep(speed);
        }

        public string ConvertTimeToString(int minutes, int seconds)
        {
            return minutes.ToString("00") + ":" + seconds.ToString("00");
        }

        public int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }











        ////Session session = new Session();
        //AvansAstrandTest avansAstrandTest = new AvansAstrandTest();

        //    SessionSnapshot currentBikeData;
        //    bool isSessionRunning = true;

        //    long startTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

        //    while (isSessionRunning)
        //    {
        //        long currentTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

        //        if (currentTime - startTime >= loopInterval)
        //        {
        //            startTime = currentTime;

        //            currentBikeData = bikeTask.GetBikeData();
        //            SessionSnapshot snapshot = new SessionSnapshot
        //            {
        //                Speed = currentBikeData.Speed,
        //                Distance = currentBikeData.Distance,
        //                Energy = currentBikeData.Energy,
        //                HeartRate = currentBikeData.HeartRate,
        //                CurrentPower = currentBikeData.CurrentPower,
        //                DateTime = DateTime.Now,
        //                Power = currentBikeData.Power,
        //                Rpm = currentBikeData.Rpm
        //            };

        //            Console.WriteLine(snapshot);

        //        }
        //    }

        //}
    }
}
