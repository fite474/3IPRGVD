using System;
using System.Collections.Generic;
using System.Text;

namespace SharedData.Data
{
    public class DoctorData : IUserData
    {
        public string Name { get; }
        public string Id { get; set; }
        public string Password { get; }
        public List<string> ClientIds { get; set; }

        public DoctorData(string name, string id, string password)
        {
            Name = name;
            this.Id = id;
            Password = password;
            ClientIds = new List<string>();
        }
    }
}
