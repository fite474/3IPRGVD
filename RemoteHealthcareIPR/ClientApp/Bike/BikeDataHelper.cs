using SharedData.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientApp.Bike
{
    class BikeDataHelper
    {
        public static SessionSnapshot AssignValues(string valueString, int power)
        {
            SessionSnapshot snapshot = new SessionSnapshot();

            string[] values = valueString.Split('\t');

            snapshot.HeartRate = int.Parse(values[0]);
            snapshot.Rpm = int.Parse(values[1]);
            snapshot.Speed = int.Parse(values[2]);
            snapshot.Distance = int.Parse(values[3]);
            snapshot.Power = int.Parse(values[4]);
            snapshot.Energy = int.Parse(values[5]);
            string[] splittedTime = values[6].Split(':');
            int timeMin = int.Parse(splittedTime[0]);
            int timeHr = int.Parse(splittedTime[1]);
            snapshot.Time = string.Format("{0:D2}:{1:D2}", timeMin, timeHr);
            snapshot.CurrentPower = power;

            return snapshot;
        }
    }
}
