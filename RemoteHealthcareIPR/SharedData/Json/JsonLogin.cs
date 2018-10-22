using System;
using System.Collections.Generic;
using System.Text;

namespace SharedData.Json
{
    class JsonLogin : IJsonData
    {

        public bool IsDoctorProgram { get; set; }
        public string Id { get; set; }
        public string Password { get; set; }

    }
}
