using System;
using System.Collections.Generic;
using System.Text;

namespace SharedData.Json
{
        class JsonRequestNewClientData : IJsonData
        {
            /// <summary>
            /// Most recent data that the doctor has
            /// </summary>
            public Dictionary<String, int> CurrentClientDatas { get; set; }
        }    
}
