using Newtonsoft.Json;
using SharedData.Data;
using SharedData.Helpers;
using SharedData.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerApp.Tasks
{
    class PatientTask : TcpTask
    {
        private readonly TcpClient client;
        private ServerData serverData;
        private PatientData currentUser;
        private List<TcpTask> connnectedTasks;
        private DoctorTask doctor;

        public PatientTask(TcpClient client, ServerData serverData, PatientData currentUser, List<TcpTask> connectedClients) : base(client, currentUser)
        {
            this.client = client;
            this.connnectedTasks = connectedClients;
            this.serverData = serverData;
            this.currentUser = currentUser;
        }

        public override void Run()
        {
            OnTryPullDoctor();
            OnHandleNextRequest();
        }

        public void OnHandleNextRequest()
        {
            TcpHelper helper = new TcpHelper(client);
            helper.ReadJsonData(OnReceivedNextRequest);
        }

        public void OnReceivedNextRequest(dynamic requestData)
        {
            Console.WriteLine("7" + requestData);
            switch ((DataType)requestData.DataType)
            {
                case DataType.BikeData:
                    SessionSnapshot snapShot = new SessionSnapshot();
                    snapShot = requestData.Data.SessionSnapshot;
                    currentUser.Sessions.Last().SessionSnapshots.Add(snapShot);
                    WriteToSessionsForUser(snapShot);
                    break;
            }
            OnHandleNextRequest();
        }

        public void OnTryPullDoctor()
        {
            bool found = false;
            while (!found)
            {
                Console.WriteLine("THID [Make client]: " + Thread.CurrentThread.ManagedThreadId);
                Thread.Sleep(200);
                doctor = GetDoctorFromClient();
                if (doctor != null)
                {
                    found = true;
                    serverData.ClientIdListLock.EnterReadLock();
                    doctor.GetClients();
                    serverData.ClientIdListLock.ExitReadLock();
                }
            }
        }

        public void SendToClient(dynamic data)
        {
            TcpHelper helper = new TcpHelper(client);
            helper.WriteJsonData(data);
        }

        private DoctorTask GetDoctorFromClient()
        {
            Console.WriteLine("Locked");
            foreach (DoctorData currentDoctor in serverData.Doctors)
            {

                foreach (string currentClient in currentDoctor.ClientIds)
                {
                    if (currentClient == currentUser.Id) // De id van de client moet al bekend zijn door het eerste bericht
                    {
                        TcpTask doc = null;
                        lock (TypeCheckTask.OnlineListSyncRoot)
                        {
                            try { doc = connnectedTasks.Find(x => x.UserData.Id == currentDoctor.Id); }// Werkt dit?
                            catch (NullReferenceException e) { e.ToString(); }

                            if (doc != null)
                            {
                                return (DoctorTask)doc;
                            }
                            else return null;
                        }
                    }
                }

            }
            Console.WriteLine("Unlocked");
            return null;
        }

        //	public List<SessionSnapshot> Read

        public void WriteToSessionsForUser(dynamic snapShot)
        {
            Task.Run(() =>
            {
                string userFile = $"UserData\\User_{ currentUser.Id}";
                PatientData clientData = JsonConvert.DeserializeObject<PatientData>(File.ReadAllText(userFile));
                clientData.Sessions.Add(snapShot);
                File.WriteAllText(userFile, JsonConvert.SerializeObject(clientData));
            });
        }

        public void ReadFromSessionsForUser()
        {
            //maybe later
        }
    }
}
