using System;
using System.Collections.Generic;
using System.Text;

namespace SharedData.Data
{
    public class Session
    {
        public PatientData PatientData { get; set; }
        public DateTime SessionStart { get; }
        public DateTime SessionEnd { get; set; }
        public List<SessionSnapshot> SessionSnapshots { get; }

        public Session(DateTime sessionStart)
        {
            SessionStart = sessionStart;
            SessionSnapshots = new List<SessionSnapshot>();
        }
    }
}
