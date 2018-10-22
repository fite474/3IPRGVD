using SharedData.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedData.Helpers
{
    class FileHelper
    {
        public static string GetClientFilePath(PatientData clientData)
        {
            return $"{ServerData.ClientDir}\\docUser_{clientData.Id}";
        }

        public static string GetDoctorFilePath(DoctorData clientData)
        {
            return $"{ServerData.DoctorDir}\\docUser_{clientData.Id}";
        }
    }
}
