using SharedData.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientApp.Bike
{
    interface IBike
    {
        SessionSnapshot ReadData();
        void PutDistance(int distance);
        void PutPower(int power);
        void PutTime(int time);
        void Change(int hr, int spd, int en);
        void Close();
    }
}
