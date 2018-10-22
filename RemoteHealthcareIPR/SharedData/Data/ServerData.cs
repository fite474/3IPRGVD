using Newtonsoft.Json;
using SharedData.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace SharedData.Data
{
    public class ServerData
    {

        public const string DoctorDir = "DoctorData";
        public const string ClientDir = "ClientData";
        public const string IdFile = "IdFile.conf";


        public List<PatientData> Clients { get; set; }
        public List<DoctorData> Doctors { get; set; }

        public ReaderWriterLockSlim ClientListLock { get; set; }
        public ReaderWriterLockSlim ClientIdListLock { get; set; }


        public ServerData()
        {
            ClientListLock = new ReaderWriterLockSlim();
            ClientIdListLock = new ReaderWriterLockSlim();
        }


        public static ServerData LoadData()
        {

            var serverData = new ServerData();
            var clientList = new List<PatientData>();
            var doctorList = new List<DoctorData>();
            serverData.Clients = clientList;
            serverData.Doctors = doctorList;

            //kijk of dirs al bestaan
            if (!Directory.Exists(ClientDir))
            {
                Directory.CreateDirectory(ClientDir);
                Directory.CreateDirectory(DoctorDir);
                MockData();
            }
            foreach (var clientFiles in Directory.GetFiles(ClientDir))
            {
                var clientData = JsonConvert.DeserializeObject<PatientData>(
                    File.ReadAllText(clientFiles)
                );
                clientList.Add(clientData);
            }
            foreach (var doctorFiles in Directory.GetFiles(DoctorDir))
            {
                var doctorData = JsonConvert.DeserializeObject<DoctorData>(
                    File.ReadAllText(doctorFiles)
                );
                doctorList.Add(doctorData);
            }
            return serverData;

        }

        private static void MockData()
        {
            PatientData c1 = new PatientData("doctor doc", IdHelper.GetIncrementedId().ToString(), "yeut", new List<Session>());
            PatientData c2 = new PatientData("doctor doc", IdHelper.GetIncrementedId().ToString(), "yeut", new List<Session>());
            File.WriteAllText($"{ClientDir}\\docUser_{c1.Id}", JsonConvert.SerializeObject(c1));
            File.WriteAllText($"{ClientDir}\\docUser_{c2.Id}", JsonConvert.SerializeObject(c2));
            DoctorData doctorData = new DoctorData("doctor doc", IdHelper.GetIncrementedId().ToString(), "yeut");
            doctorData.ClientIds.Add(c1.Id);
            doctorData.ClientIds.Add(c2.Id);
            File.WriteAllText($"{DoctorDir}\\docUser_{doctorData.Id}", JsonConvert.SerializeObject(doctorData));

        }
    }
}
