using ClientServerUtil;
using DoctorApp;
using PatientApp.Bike;
using PatientApp.Gui;
using SharedData.Data;
using SharedData.Json;
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
using System.Windows.Forms;

namespace PatientApp.Bike
{
    public class BikeConnection
    {
        public delegate void ReceiveResponse(Datagram jsonResponse);


        private static TcpClient Client;
        Thread ReadThread;
        Thread SendThread;
        NetworkStream stream;
        public event ReceiveResponse OnReceiveResponse;
        private List<int> heartbeatList;

        private BikeTask bikeTask = new BikeTask();
        private long loopInterval = 1000; //In milliseconds
        private int timeMinutes = 7;
        private int timeSeconds = 0;
        private Session thisSession;
        private readonly int speed = 1000;
        public string Age { get; set; }
        public string Weight { get; set; }
        public string Gender { get; set; }
        private int power;

        private double maxHeartFrequentie = 200.0;
        private int roundsPerMin = 0;
        private ServerConnection connection;
        public string CurrentTimeString { get; set; }
        PatientTestInstructions patientTestInstructions = new PatientTestInstructions();
        private bool steadyState = true;
        //private SessionSnapshot currentData;
        //private IBike bike;
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

            
            connection = new ServerConnection(false);
            connection.OnReceiveResponse += ResponseHandler;
            OnReceiveResponse += handleResponse;
            patientTestInstructions = new PatientTestInstructions();
            //patientTestInstructions.ShowDialog();
            string text = "Wacht tot de dokter de sessie start!";
            MessageBox.Show(text);
        }

        public void RunBikeLoop(object o)
        {
            //connect();
            heartbeatList = new List<int>();
            SetMaxHF(Age);

            thisSession = new Session(DateTime.Now);
            PatientData c1 = new PatientData("doctor doc", "1", "yeut", new List<Session>());
            thisSession.PatientData = c1;
            thisSession.SessionEnd = DateTime.Now.AddMinutes(7.0);

            power = 25;
            patientTestInstructions.setPower("Current Power is: " + power + "Wattt");
            setPhase("Warming up");
            DateTime startTime = DateTime.Now;
            //bool wait = false;
            while (timeMinutes >= 6)//5)
            {
                //DateTime currentTime = DateTime.Now;
                if (timeSeconds % 5 == 0)//((currentTime - startTime).Ticks / TimeSpan.TicksPerSecond >= 1 )//|| !wait)
                {
                    
                    if (roundsPerMin < 50 && power > 25)
                    {
                        power -= 5;
                        bikeTask.IncreasePower(power);
                        patientTestInstructions.setPower("current Power is: " + power + "Watt");
                    }

                    else if (roundsPerMin > 60 && power < 400)
                    {
                        power += 5;
                        bikeTask.IncreasePower(power);
                        patientTestInstructions.setPower("current Power is: " + power + "Watt");
                    }
                    


                    //CycleRun(power, ConvertTimeToString(timeMinutes, timeSeconds));
                }
                CycleRun(power, ConvertTimeToString(timeMinutes, timeSeconds));
                
            }

            //SendLogFile(thisSession);
            setPhase("Astrand test");
            //startTime = DateTime.Now;
           // wait = false;
            while (timeMinutes >= 1)
            {
                //DateTime currentTime = DateTime.Now;
                if (timeSeconds % 5 == 0)//if ((currentTime - startTime).Ticks / TimeSpan.TicksPerSecond >= 5 )//|| !wait)
                {
                    if (bikeTask.GetBikeData().HeartRate < 100 && power < 400)
                    {
                        power += 5;
                        patientTestInstructions.setPower("current Power is: " + power + "Watt");
                        bikeTask.IncreasePower(power);
                        // wait = true;
                    }
                    else if (bikeTask.GetBikeData().HeartRate > maxHeartFrequentie || roundsPerMin < 40 && power > 25)
                    {
                        power -= 5;
                        patientTestInstructions.setPower("current Power is: " + power + "Watt");
                        bikeTask.IncreasePower(power);
                        // wait = true;
                    }
                    //startTime = DateTime.Now;


                }
                    CycleRun(power, ConvertTimeToString(timeMinutes, timeSeconds));
                
            }
            setPhase("Cooling down");
            while (timeMinutes >= 0)
            {
                CycleRun(power, ConvertTimeToString(timeMinutes, timeSeconds));
            }
            setPhase("Done");
            int heartbeatAvr = (int)heartbeatList.Average();
            CalculateVO2Max(heartbeatAvr);
            SendLogFile(thisSession);
        }

        public void CycleRun(int currentPower, string time)//int heartbeat, int currentPower, string time)
        {
            SessionSnapshot currentBikeData = bikeTask.GetBikeData();
            bool secondsIsZero = timeSeconds == 0;
            //bikeTask.IncreasePower(currentPower);
            //heartbeat = currentBikeData.HeartRate;

           // Console.WriteLine("heartbeat is: "+ heartbeat);

            roundsPerMin = currentBikeData.Rpm;

            if (timeMinutes < 8)//2)
            {
                patientTestInstructions.setHeartbeatLabelvisible();
                if (timeSeconds % 15 == 0)
                {
                    SessionSnapshot ss = new SessionSnapshot();
                    ss.CurrentPower = currentPower;
                    ss.Time = time;
                    ss.HeartRate = currentBikeData.HeartRate;
                    ss.Rpm = currentBikeData.Rpm;
                    ss.Speed = currentBikeData.Speed;
                    ss.Distance = currentBikeData.Distance;
                    ss.Energy = currentBikeData.Energy;
                    //maak gemiddelde van heartbeat
                    MakeAvarageHeartbeat(currentBikeData.HeartRate);
                    //MakeAvarageHeartbeat(heartbeat);
                    SendSnap(ss);
                    //Session.SessionSnapshots.Add(ss);
                }
            }
            else
            {
                //if (timeSeconds == 0)
                //{
                    SessionSnapshot ss = new SessionSnapshot();
                    ss.CurrentPower = currentPower;
                    ss.Time = time;
                    ss.HeartRate = currentBikeData.HeartRate;
                    ss.Rpm = currentBikeData.Rpm;
                    ss.Speed = currentBikeData.Speed;
                    ss.Distance = currentBikeData.Distance;
                    ss.Energy = currentBikeData.Energy;
                    SendSnap(ss);
                    //Session.SessionSnapshots.Add(ss);
                //}
            }

            if (timeSeconds == 0)
            {
                timeSeconds = 59;
                timeMinutes--;
            }
            else timeSeconds -= 1;//else timeSeconds--;//else timeSeconds-=10;
            Console.WriteLine("timeSeconds: " + timeSeconds);

            if (currentBikeData.HeartRate > maxHeartFrequentie)//heartbeat > maxHeartFrequentie)
            {
                //stop programma
            }

            //SendSnap(ss);
            setGUILabels(currentBikeData.Rpm, currentBikeData.HeartRate);//roundsPerMin, currentBikeData.HeartRate);


            DateTime startTime = DateTime.Now;
            SessionSnapshot oldData = currentBikeData;
            bool waitForData = true;
            while (waitForData)
            {
                //DateTime currentTime = DateTime.Now;
                //if ((currentTime - startTime).Ticks / TimeSpan.TicksPerSecond >= 1)
                //{
                    //startTime = currentTime;
                    currentBikeData = bikeTask.GetBikeData();

                    if (oldData != currentBikeData)
                    waitForData = false;
                //}
            }
            // Thread.Sleep(speed);

        }

        private void SendSnap(SessionSnapshot snap)
        {
            Datagram datagram = new Datagram();
            datagram.DataType = DataType.SessionSnapshot;
            datagram.Data = snap;
            thisSession.SessionSnapshots.Add(snap);
            connection.SendData(datagram);
        }
        private void SendLogFile(Session session)
        {
            Datagram datagram = new Datagram();
            datagram.DataType = DataType.LogData;
            datagram.Data = session;
            connection.SendData(datagram);
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



        private void setPhase(string phase)
        {
            patientTestInstructions.setPhaseLabel(phase);
        }

        private void setGUILabels(int rpm, int heartbeat)
        {
            if (rpm < 50)
            { patientTestInstructions.setInstructionLabel("Fiets sneller " + rpm); }
            else if (rpm > 65)
            { patientTestInstructions.setInstructionLabel("Fiets rustiger " + rpm); }
            else
            { patientTestInstructions.setInstructionLabel("Goed bezig, houd dit tempo aan " + rpm); }

            patientTestInstructions.setHeartbeatLabel("Current heartbeat is: " + heartbeat + "");
            //patientTestInstructions.set

            patientTestInstructions.setSteadyStateLabel(steadyState);
            Console.WriteLine("rpm: "+rpm);
        }


        private void connect()
        {
            byte[] data = new byte[1024];
            Client = new TcpClient(IPAddress.Loopback.ToString(), 667);
            stream = Client.GetStream();
            //byte[] messageBytes = Util.BuildJSON(data);
        }

        private void ResponseHandler(Datagram jsonResponse)
        {
            Task.Run(() => OnReceiveResponse?.Invoke(jsonResponse));
        }

        public void handleResponse(Datagram data)
        {
            switch (data.DataType)
            {
                case DataType.StartSession:
                    {
                        new Thread(UIStart).Start();
                        new Thread(RunBikeLoop).Start();
                        break;
                    }

                    //recievedata and start if correct


            }
        }

        private void UIStart(object o)
        {
            patientTestInstructions.ShowDialog();
        }



        private double CalculateVO2Max(int averageHeartRate)
        {
            int tempGend = 0;
            if (Gender.Equals("Male"))
            {tempGend = 1;}

            if ((averageHeartRate - 73 - (tempGend * 10)) == 0)
            {averageHeartRate += 1;}

            double vo2 = (1.8 * averageHeartRate) / int.Parse(Weight);
            double vo2Max = vo2 * ((220 - int.Parse(Age) - 73 - (tempGend * 10)) / (averageHeartRate - 73 - (tempGend * 10)));
            Console.WriteLine("VO2Max clc: " + vo2Max);
            Math.Round(vo2Max, 2);
            if (steadyState)
            {
                patientTestInstructions.setvo2MaxLabel("VO2Max: " + vo2Max + " L/min");
            }
            else
            {
                patientTestInstructions.setvo2MaxLabel("Geen steady state bereikt, VO2Max is niet betrouwbaar \nVO2Max: " + vo2Max + " L/min");
            }
            
            return vo2Max;

        }

        private void MakeAvarageHeartbeat(int heartbeat)
        {
            if (heartbeat > 5)
            {
                heartbeatList.Add(heartbeat);
            }
            if (heartbeatList.Count > 2)
            {
                int maxValue = 0;
                int minValue = 200;
                foreach (int ritme in heartbeatList)
                {
                    if (ritme > maxValue)
                    { maxValue = ritme; }
                    if (ritme < minValue)
                    { minValue = ritme; }
                    
                }

                if ((maxValue - minValue) > 30)
                {
                    steadyState = false;
                    

                }
            }
        }
    }
}
