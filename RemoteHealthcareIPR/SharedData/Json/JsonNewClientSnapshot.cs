using SharedData.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedData.Json
{
    class JsonNewClientSnapshot : IJsonData
    {
        public Dictionary<String, List<SessionSnapshot>> NewSnapShots { get; set; }

    }
}
