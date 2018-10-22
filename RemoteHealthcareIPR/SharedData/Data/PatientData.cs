using System;
using System.Collections.Generic;
using System.Text;

namespace SharedData.Data
{
    public class PatientData : IUserData
    {
        public string Name { get; }
        public string Id { get; }
        public string Password { get; }
        public List<Session> Sessions { get; set; }

        public PatientData(string name, string id, string password, List<Session> sessions)
        {
            Name = name;
            Id = id;
            Password = password;
            Sessions = sessions;
        }
    }
}
