using SharedData.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PatientApp.Bike
{
    public delegate void BikeDataReceived(SessionSnapshot bikeData);

    public class BikeTask
    {

        private Thread thread;
        private SessionSnapshot currentData;
        private bool threadRunning;
        private IBike bike;
        private object bikeLockObject;
        Random random = new Random();

        public event BikeDataReceived OnBikeDataReceived;

        public BikeTask()
        {
            this.bikeLockObject = new object();
            this.bike = new BikeSimulator();
        }

        public void SetBikeEvent(BikeDataReceived dataReceived)
        {
            OnBikeDataReceived += dataReceived;
        }
        
        public void ChangeBike(string comPort)
        {
            if (threadRunning)
            {
                bike.Close();
            }
            bike = new BikeKettler(comPort);
            if (!threadRunning)
            {
                thread = new Thread(StartBikeSession);
                thread.Start();
            }

        }

        public void UseSimulator()
        {
            if (threadRunning)
            {
                bike.Close();
            }
            bike = new BikeSimulator();
            if (!threadRunning)
            {
                thread = new Thread(StartBikeSession);
                thread.Start();
            }
        }

        public SessionSnapshot GetBikeData()
        {
            return currentData;
        }

        public void IncreaseDistance()
        {
            lock (bikeLockObject)
            {
                currentData = bike.ReadData();
                bike.PutDistance(currentData.Distance + 1000);
            }
        }

        public void DecreaseDistance()
        {
            lock (bikeLockObject)
            {
                currentData = bike.ReadData();
                bike.PutDistance(currentData.Distance - 1000);
            }
        }

        public void IncreasePower(int power)
        {
            lock (bikeLockObject)
            {
                currentData = bike.ReadData();
                bike.PutPower(currentData.Power + power);
            }
        }

        public void DecreasePower(int power)
        {
            lock (bikeLockObject)
            {
                currentData = bike.ReadData();
                bike.PutPower(currentData.Power - power);
            }
        }

        public void IncreaseTime()
        {
            lock (bikeLockObject)
            {
                currentData = bike.ReadData();
                Console.WriteLine(currentData.Time);
                bike.PutTime(EncodeTime(currentData.Time, 60));
            }
        }

        public void DecreaseTime()
        {
            lock (bikeLockObject)
            {
                currentData = bike.ReadData();
                bike.PutTime(EncodeTime(currentData.Time, -60));
            }
        }
        
        private int EncodeTime(string time, int increaseOrDecrease)
        {
            string[] timeSplit = time.Split(':');
            int minutesToSeconds = int.Parse(timeSplit[0]) * 60;
            return (int.Parse(timeSplit[1]) + minutesToSeconds) + increaseOrDecrease;
        }

        private void StartBikeSession()
        {
            DateTime startTime = DateTime.Now;
            bool running = true;
            while (running)
            {
                DateTime currentTime = DateTime.Now;
                if ((currentTime - startTime).Ticks / TimeSpan.TicksPerSecond >= 1)
                {
                    startTime = currentTime;
                    currentData = bike.ReadData();

                    if (currentData == null)
                        continue;

                    OnBikeDataReceived?.Invoke(currentData);
                    if (bike is BikeSimulator)
                    {
                        ChangeBikeValues();
                    }
                }
            }
        }

        private void ChangeBikeValues()
        {
            bike.PutTime(EncodeTime(currentData.Time, -1));
            int r = random.Next(0, 4);
            if (r == 1)
            {
                bike.Change(random.Next(1, 5), random.Next(1, 5), random.Next(1, 5));
            }
            else if (r == 0)
            {
                bike.Change(-1 * random.Next(1, 5), -1 * random.Next(1, 5), -1 * random.Next(1, 5));
            }
            else
            {
                bike.Change(0, 0, 0);
            }
        }
    }
}


