using ClientServerUtil;
using DoctorApp;
using PatientApp.Bike;
using PatientApp.Gui;
using SharedData.Data;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PatientApp.Bike
{
    public class BikeConnection
    {
        public delegate void ReceiveResponse(SessionSnapshot jsonResponse);


        private static TcpClient Client;
        Thread ReadThread;
        Thread SendThread;
        NetworkStream stream;
        public event ReceiveResponse OnReceiveResponse;

        private BikeTask bikeTask = new BikeTask();
        private long loopInterval = 1000; //In milliseconds
        private int timeMinutes = 7;
        private int timeSeconds = 0;
        //private Session Session { get; }
        private readonly int speed = 100;
        public string Age { get; set; }
        public string Weight { get; set; }
        public string Gender { get; set; }
        private int power;
        private int heartbeat = 0;
        private double maxHeartFrequentie = 200.0;
        private int roundsPerMin = 0;
        private ServerConnection connection;
        public string CurrentTimeString { get; set; }
        PatientTestInstructions patientTestInstructions = new PatientTestInstructions();

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

        public void RunTestGUI(object o)
        {
            connection = new ServerConnection();
            connection.OnReceiveResponse += handleResponse;
            //patientTestInstructions = new PatientTestInstructions();
            //patientTestInstructions.ShowDialog();
        }

        public void RunBikeLoop(object o)
        {
            connect();

            SetMaxHF(Age);



            power = 40;

            while (timeMinutes >= 5)
            {
                if (roundsPerMin < 50)
                { power -= 5; }
                if (roundsPerMin > 60)
                { power += 5; }
                CycleRun(power, ConvertTimeToString(timeMinutes, timeSeconds));
            }
            while (timeMinutes >= 1)
            {
                if (heartbeat < 130)
                { power += 5; }
                else if (heartbeat > maxHeartFrequentie)
                { power -= 5; }

                CycleRun(power, ConvertTimeToString(timeMinutes, timeSeconds));
            }
            while (timeMinutes >= 0)
            {
                CycleRun(power, ConvertTimeToString(timeMinutes, timeSeconds));
            }
        }

        public void CycleRun(int currentPower, string time)//int heartbeat, int currentPower, string time)
        {
            SessionSnapshot currentBikeData = bikeTask.GetBikeData();
            bool secondsIsZero = timeSeconds == 0;
            bikeTask.IncreasePower(currentPower);
            heartbeat = currentBikeData.HeartRate;
            roundsPerMin = currentBikeData.Rpm;

            if (timeMinutes < 2)
            {
                if (timeSeconds % 15 == 0)
                {
                    SessionSnapshot ss = new SessionSnapshot();
                    ss.CurrentPower = currentPower;
                    ss.Time = time;
                    ss.HeartRate = heartbeat;
                    ss.Rpm = currentBikeData.Rpm;
                    ss.Speed = currentBikeData.Speed;
                    ss.Distance = currentBikeData.Distance;
                    ss.Energy = currentBikeData.Energy;

                    //maak gemiddelde van heartbeat

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
                    ss.Rpm = currentBikeData.Rpm;
                    ss.Speed = currentBikeData.Speed;
                    ss.Distance = currentBikeData.Distance;
                    ss.Energy = currentBikeData.Energy;
                    //Session.SessionSnapshots.Add(ss);
                }
            }

            if (timeSeconds == 0)
            {
                timeSeconds = 59;
                timeMinutes--;
            }
            else timeSeconds--;

            if (heartbeat > maxHeartFrequentie)
            {
                //stop programma
            }
            setInstructions(roundsPerMin);
            Thread.Sleep(speed);

        }

        public string ConvertTimeToString(int minutes, int seconds)
        {
            CurrentTimeString = minutes.ToString("00") + ":" + seconds.ToString("00");
            patientTestInstructions.setTimeLabel(CurrentTimeString);
            return CurrentTimeString;
        }

        public int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        private void SetMaxHF(string age)
        {
            double localAge = int.Parse(age);
            double hfFactor = 1.0;
            if (localAge < 25)
            { hfFactor = 1.12; }

            if ( 40 > localAge && localAge > 35)
            { hfFactor = 0.93; }

            if (45 > localAge && localAge > 40)
            { hfFactor = 0.83; }

            if (50 > localAge && localAge > 45)
            { hfFactor = 0.75; }

            if (55 > localAge && localAge > 50)
            { hfFactor = 0.69; }

            if (40 > localAge && localAge > 55)
            { hfFactor = 0.64; }

            maxHeartFrequentie = maxHeartFrequentie * hfFactor;
        }

        private void setInstructions(int rpm)
        {
            if (rpm < 50)
            { patientTestInstructions.setInstructionLabel("fiets sneller"); }
            else if (rpm > 65)
            { patientTestInstructions.setInstructionLabel("fiets rustiger"); }
            else
            { patientTestInstructions.setInstructionLabel("goed bezig, houd dit tempo aan"); }
        }

        private void connect()
        {
            byte[] data = new byte[1024];
            Client = new TcpClient(IPAddress.Loopback.ToString(), 667);
            stream = Client.GetStream();
            //byte[] messageBytes = Util.BuildJSON(data);
        }

        private void ResponseHandler(SessionSnapshot jsonResponse)
        {
            Task.Run(() => OnReceiveResponse?.Invoke(jsonResponse));
        }

        public void handleResponse(SessionSnapshot snap)
        {
            //recievedata and start if correct
            //patientTestInstructions = new PatientTestInstructions();
            //patientTestInstructions.ShowDialog();
            //new Thread(RunBikeLoop).Start();

        }
    }
}
