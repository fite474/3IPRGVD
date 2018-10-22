using SharedData.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedData.Json
{
    class JsonNewClientData : IJsonData
    {
        public PatientData ClientData { get; set; }
    }
}
