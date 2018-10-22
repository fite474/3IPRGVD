using System;
using System.Collections.Generic;
using System.Text;

namespace SharedData.Data
{
    public class SessionSnapshot
    {
        public DateTime DateTime { get; set; }
        public string Time { get; set; }
        public int HeartRate { get; set; }
        public int Rpm { get; set; }
        public int Speed { get; set; }
        public int Distance { get; set; }
        public int Power { get; set; }
        public int Energy { get; set; }
        public int CurrentPower { get; set; }

        public override string ToString()
        {
            return "" +
               "Heartrate: " + HeartRate +
               "\nDistance: " + Distance;
        }
    }
}
