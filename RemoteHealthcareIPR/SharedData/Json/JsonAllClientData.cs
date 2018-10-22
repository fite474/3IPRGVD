using SharedData.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedData.Json
{
    class JsonAllClientData : IJsonData
    {
        public List<PatientData> ClientDatas { get; set; }
    }
}
