using SharedData.Data;
using SharedData.Helpers;
using SharedData.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp.Tasks
{
    class TypeCheckTask
    {
        public static Object OnlineListSyncRoot = new object();
        private List<TcpTask> currentTasks;
        private readonly TcpClient client;
        private ServerData serverData;

        public TypeCheckTask(TcpClient client, List<TcpTask> currentTasks, ServerData serverData)
        {
            this.client = client;
            this.serverData = serverData;
            this.currentTasks = currentTasks;
            TcpHelper helper = new TcpHelper(client);
            helper.ReadJsonData(OnAuthorizeConnection);
        }

        public void OnAuthorizeConnection(Datagram received)
        {
            TcpTask currentTask = null;
            if (received.DataType == DataType.Login)
            {
                var loginRequest = TcpHelper.ToConcreteType<JsonLogin>(received.Data);
                if (loginRequest.IsDoctorProgram)
                {
                    DoctorData currentUser = FindDoctorWithPassword(loginRequest.Id, loginRequest.Password);
                    if (currentUser != null)
                    {
                        DoctorTask doctorTask = new DoctorTask(client, serverData, currentUser, currentTasks);
                        currentTask = doctorTask;
                        lock (OnlineListSyncRoot)
                        {
                            currentTasks.Add(doctorTask);
                        }
                    }
                }
                else
                {
                    //moet voor client lists in server data want we voegen data toe maar lezen ook.
                    serverData.ClientListLock.EnterReadLock();
                    PatientData currentUser = FindClientWithPassword(loginRequest.Id, loginRequest.Password);
                    serverData.ClientListLock.ExitReadLock();
                    if (currentUser != null)
                    {
                        // ZORGT VOOR DEADLOCK!!!
                        PatientTask clientTask = new PatientTask(client, serverData, currentUser, currentTasks);
                        currentTask = clientTask;
                        lock (OnlineListSyncRoot)
                        {
                            currentTasks.Add(clientTask);
                        }
                    }
                }
            }

            //TcpHelper writerHelper = new TcpHelper(client);
            //dynamic response = new ExpandoObject();
            //response.DataType = DataType.Login;
            //response.Data = new ExpandoObject();

            TcpHelper writerHelper = new TcpHelper(client);
            var response = new Datagram();
            response.DataType = DataType.Login;
            if (currentTask != null)
            {
                response.Data = new JsonResponse
                {
                    Error = "200",
                    Message = "LoginOK"
                };
                writerHelper.WriteJsonData(response);
                currentTask.Run();
            }
            else
            {
                response.Data = new JsonResponse
                {
                    Error = "500",
                    Message = "LoginWrong"
                };
                writerHelper.WriteJsonData(response);
                //lets try that again
                TcpHelper helper = new TcpHelper(client);
                helper.ReadJsonData(OnAuthorizeConnection);
            }

        }



        public DoctorData FindDoctorWithPassword(string id, string password)
        {
            Console.WriteLine(id);
            return serverData.Doctors.Where(x => x.Id == id && x.Password == password)?.FirstOrDefault();
        }

        public PatientData FindClientWithPassword(string id, string password)
        {
            return serverData.Clients.Where(x => x.Id == id && x.Password == password)?.FirstOrDefault();
        }
    }
}
