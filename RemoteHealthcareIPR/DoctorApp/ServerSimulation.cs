using SharedData.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DoctorApp
{
    class ServerSimulation
    {
        private int timeMinutes = 7;
        private int timeSeconds = 0;
        private Session Session { get; }
        private readonly int speed = 100;

        public ServerSimulation(Session session)
        {
            this.Session = session;
            StartCycleTest();
        }

        public void StartCycleTest()
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
                    Session.SessionSnapshots.Add(ss);
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
                    Session.SessionSnapshots.Add(ss);
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
    }
}