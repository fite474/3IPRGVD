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
using System.Threading.Tasks;

namespace ServerApp.Tasks
{
    class DoctorTask : TcpTask
    {
        private TcpClient client;
        private ServerData serverData;
        private DoctorData doctor;
        private List<TcpTask> currentTasks;
        private List<PatientTask> clientsFromDoctor = new List<PatientTask>();

        public DoctorTask(TcpClient client, ServerData serverData, DoctorData doctor, List<TcpTask> currentTasks) : base(client, doctor)
        {
            this.client = client;
            this.serverData = serverData;
            this.doctor = doctor;
            this.currentTasks = currentTasks;
        }

        public override void Run()
        {
            GetClients();
            OnHandleNextRequest();
        }


        public void OnHandleNextRequest()
        {
            TcpHelper helper = new TcpHelper(client);
            helper.ReadJsonData(OnReceivedNextRequest);
        }

        public void OnReceivedNextRequest(Datagram requestData)
        {
            switch (requestData.DataType)
            {
                case DataType.Message:
                case DataType.ChangeBike:
                case DataType.EmergencyBreak:
                    string id = requestData.Data.RecipientId;
                    if (id == "All")                                     // NEEDS TO BE TESTED // HIER KAN IETS FOUT GAAN
                    {
                        foreach (PatientTask ct in clientsFromDoctor)
                        {
                            GetClient(ct.UserData.Id).SendToClient(requestData);
                        }
                    }
                    else
                    {
                        GetClient(id).SendToClient(requestData);
                    }                                                //HIER EINDIGT HET
                    break;
                case DataType.AddClient:
                    Console.WriteLine(requestData.Data);
                    {
                        var clientData = requestData.Data;
                        WriteNewClientData(clientData);
                        Datagram response = new Datagram();
                        response.DataType = DataType.NewClientData;
                        response.Data = new JsonNewClientData { ClientData = serverData.Clients.Last() };
                        SendToDoctor(response);
                    }

                    break;
                case DataType.StartSession:
                    {
                        string clientId = requestData.Data.Id;
                        PatientTask task = GetClient(clientId);
                        if (clientsFromDoctor.Contains(task))
                        {
                            Datagram response = new Datagram();
                            response.DataType = DataType.StartSession;
                            response.Data = new JsonResponse { Error = "200", Message = "SessionStart" };
                            task.SendToClient(response);
                            SendToDoctor(response);
                        }
                    }
                    break;
                case DataType.EndSession:
                    {
                        string clientId = requestData.Data.Id;
                        PatientTask task = GetClient(clientId);
                        Datagram response = new Datagram();
                        response.DataType = DataType.EndSession;
                        response.Data = new JsonResponse { Error = "200", Message = "SessionEnd" };
                        task.SendToClient(response);
                        SendToDoctor(response);
                    }
                    break;
                case DataType.RequestAllClientData:

                    SendToDoctor(new Datagram()
                    {
                        DataType = DataType.AllClientData,
                        Data = new JsonAllClientData()
                        {
                            ClientDatas = serverData.Clients
                        }
                    });
                    break;
                case DataType.RequestNewClientSnapshots:
                    JsonRequestNewClientData d = TcpHelper.ToConcreteType<JsonRequestNewClientData>(requestData.Data);

                    var snapshots = GetAllNewSnapshots(d.CurrentClientDatas);

                    SendToDoctor(new Datagram()
                    {
                        DataType = DataType.NewClientSnapshots,
                        Data = new JsonNewClientSnapshot()
                        {
                            NewSnapShots = snapshots
                        }
                    });

                    break;
                case DataType.LogOut:
                    foreach (PatientTask ct in clientsFromDoctor)
                    {
                        ct.SendToClient(new Datagram()
                        {
                            DataType = DataType.LogOut,
                            Data = new JsonLogOut() { }
                        });
                    }
                    currentTasks.RemoveAll((x) => x.Client == client);
                    foreach (PatientTask ct in clientsFromDoctor)
                    {
                        ct.Client.Close();
                    }
                    client.Close();
                    break;

            }
            OnHandleNextRequest();
        }


        private Dictionary<string, List<SessionSnapshot>> GetAllNewSnapshots(Dictionary<String, int> currentClientDatas)
        {

            Dictionary<string, List<SessionSnapshot>> allNewSnapshots = new Dictionary<string, List<SessionSnapshot>>();
            foreach (KeyValuePair<string, int> client in currentClientDatas)
            {
                allNewSnapshots.Add(client.Key, GetNewSnapshots(client.Key, client.Value));
            }

            return allNewSnapshots;
        }

        private List<SessionSnapshot> GetNewSnapshots(string clientId, int amountOfSnapshots)
        {
            List<SessionSnapshot> newSnapshots = new List<SessionSnapshot>();

            PatientData clientData = (PatientData)GetClient(clientId).UserData;
            Session session = clientData.Sessions.Last();
            int amountOfNewSnapshots = session.SessionSnapshots.Count - amountOfSnapshots;

            for (int i = 1; i <= amountOfNewSnapshots; i++)
            {
                newSnapshots.Add(session.SessionSnapshots[session.SessionSnapshots.Count - i]);
            }

            return newSnapshots;
        }


        private PatientTask GetClient(string id)
        {
            Console.WriteLine("client target id = " + id);
            foreach (PatientTask clientFromDoctor in clientsFromDoctor)
            {
                Console.WriteLine("availible clients : " + clientFromDoctor.UserData.Id);
                if (clientFromDoctor.UserData.Id == id)
                {
                    return clientFromDoctor;
                }
            }
            return null;
        }

        public void GetClients()
        {
            try
            {

                foreach (TcpTask tt in currentTasks)
                {
                    string currentId = tt.UserData.Id;
                    if (
                        doctor.ClientIds.Contains(currentId) && // id zit in de lijst clientIds
                        !clientsFromDoctor.Contains(tt) && // zit nog niet in de lijst currentUsers
                        tt is PatientTask // het is een client

                    )
                    {
                        clientsFromDoctor.Add((PatientTask)tt);

                        Console.WriteLine(clientsFromDoctor.Count);
                    }

                }
            }
            catch (NullReferenceException e) { e.ToString(); }
            catch (InvalidOperationException e) { e.ToString(); }
        }


        public void WriteNewClientData(dynamic newClient)
        {
            int id = IdHelper.GetIncrementedId();
            serverData.ClientListLock.EnterWriteLock();
            PatientData dataToAdd = new PatientData((string)newClient.Username, id.ToString(), (string)newClient.Password, new List<Session>());
            serverData.Clients.Add(dataToAdd);
            serverData.ClientIdListLock.EnterWriteLock();
            doctor.ClientIds.Add(dataToAdd.Id);
            serverData.ClientIdListLock.ExitWriteLock();
            serverData.ClientListLock.ExitWriteLock();
            File.WriteAllText(FileHelper.GetClientFilePath(dataToAdd), JsonConvert.SerializeObject(dataToAdd));
            File.WriteAllText(FileHelper.GetDoctorFilePath(doctor), JsonConvert.SerializeObject(doctor));
        }

        public void SendToDoctor(Datagram data)
        {
            TcpHelper helper = new TcpHelper(client);
            Console.WriteLine("verstuurd naar doctor: " + data);
            helper.WriteJsonData(data);
        }
    }
}
